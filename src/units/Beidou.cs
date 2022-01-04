using System;
using System.Collections.Generic;
using Tcc.elements;
using Tcc.events;
using Tcc.stats;
using Tcc.weapons;

namespace Tcc.units
{
    public class Beidou: Unit
    {
        private SnapshottedStats burstSnapshot;
        private HitType skillType, bounceType, burstType;
        private Guid c6debuff = Guid.NewGuid();
        private double lastProc = -1;
        public Beidou(int constellationLevel = 0, string level = "90", int autoLevel = 6, int skillLevel = 6,
            int burstLevel = 6) :
            base("beidou", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.ELECTRO,
                WeaponType.CLAYMORE)
        {
            SkillICD = new ICDCreator(5, 4);
            skillType = new HitType(Element.ELECTRO, gauge:2);
            // TODO: find out what the time between procs is
            int bounces = constellationLevel < 2 ? 3 : 5;
            bounceType = new HitType(Element.ELECTRO, false, bounces, true, icd: new ICDCreator(2.5, 3), delay: 0.1);
            burstType = new HitType(Element.ELECTRO, gauge: 4);
        }

        // TODO: add c4 bullshit but no one autos with beidou anyway so who cares
        public override List<WorldEvent> Skill(double timestamp, params object[] p)
        {   
            int chargeLevel = p[0] as int? ?? 0;
            // assert this is less than 2
            double mvs = StartingAbilityStats[Types.SKILL].GetMotionValue(0) +
                         chargeLevel * StartingAbilityStats[Types.SKILL].GetMotionValue(1);
            return new List<WorldEvent>
            {
                new Hit(timestamp + 41/60f, mvs, GetStatsPage, this, Types.SKILL, skillType, $"Tidecaller charge level {chargeLevel}")
            };
        }

        public override void Reset()
        {
            base.Reset();
            lastProc = -1;

        }

        public override List<WorldEvent> Burst(double timestamp)
        {
            List<WorldEvent> events = new List<WorldEvent>
            {
                BurstActivated(timestamp),
                new Hit(timestamp + 45/60f, 0, burstSnapshot.GetStats, this, Types.BURST, burstType, "Stormbreaker cast")
                
            };
            events.Add(new WorldEvent(timestamp, world =>
            {
                foreach (var unit in world.GetUnits())
                {
                    if (unit is not null) unit.NormalAttackHook += Bounce;
                }
            }));
            events.Add(new WorldEvent(timestamp + 15, world =>
            {
                foreach (var unit in world.GetUnits())
                {
                    if (unit is not null) unit.NormalAttackHook += Bounce;
                }
            }));
            if (ConstellationLevel == 6)
            {
                // do debuff here
            }
            return events;
        }

        // does this proc on normal and auto
        public void Bounce(object? sender, NormalAttackArgs e)
        {
            
            double timestamp = e.Timestamp + e.Duration;
            if (timestamp - lastProc < 1)
            {
                return;
            }

            lastProc = timestamp;

            e.World.AddWorldEvent(new Hit(timestamp, 1, burstSnapshot.GetStats, this, Types.BURST, bounceType, "Strombreaker bounce"));
        }
    }
}