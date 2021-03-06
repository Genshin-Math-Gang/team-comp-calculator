using System;
using System.Collections.Generic;
using Tcc.buffs;
using Tcc.elements;
using Tcc.events;
using Tcc.stats;
using Tcc.weapons;

namespace Tcc.units
{
    public class Raiden: Unit
    {
        static readonly Guid SKILL_BUFF_ID = new Guid("0e88cab3-e1d1-4592-9059-b36e6595e25d");
        static readonly double BUFF_DURATION = (25);
        // raiden will need more ICD stuff but i need to find that

        private readonly HitType SkillHitType;

        EventHandler<(Unit from, Unit to, double)> currentBuffListener = null;

        public Raiden(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("raiden", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.ELECTRO, WeaponType.POLEARM) 
        {
            AutoAttackFrameData = new[] {0, 14, 31, 56, 102, 151, 172, 44};
            int[] AutoAttackBurstFrameData = new[] {0, 12, 32, 54, 95, 139, 215, 50};
            SkillICD = new();
            SkillHitType = new HitType(Element.ELECTRO, true, icd: SkillICD);
        }

        public override List<WorldEvent> Skill(double timestamp, params object[] p)
        {
            var events = new List<WorldEvent>();
            var expiryTime = timestamp + BUFF_DURATION; //Bugged because it was static

            EventHandler<(Unit from, Unit to, double)> newBuffListener = null;

            events.Add(new WorldEvent(timestamp, (world) =>
            {
                world.unitSwapped -= currentBuffListener;

                this.RemoveAllBuffs(SKILL_BUFF_ID);
                this.AddBuff(CreateSkillBuff(expiryTime), Types.BURST);

                newBuffListener = (_, data) =>
                {
                    data.from.RemoveAllBuffs(SKILL_BUFF_ID);
                    data.to.AddBuff(CreateSkillBuff(expiryTime), Types.BURST);
                };

                world.unitSwapped += newBuffListener;
                currentBuffListener = newBuffListener;
            }));

            events.Add(new WorldEvent(expiryTime, (world) => world.unitSwapped -= newBuffListener));
            events.Add(new Hit(timestamp, 0, GetStatsPage, this, Types.SKILL, SkillHitType));

            return events;
        }

        public override List<WorldEvent> Burst(double timestamp)
        {
            throw new NotImplementedException();
        }

        private Buff<AbilityModifier> CreateSkillBuff(double expiryTime)
        {
            return new RefreshableBuff<AbilityModifier>(
                SKILL_BUFF_ID,
                expiryTime: expiryTime,
                data => new 
                StatsPage(Stats.DamagePercent, 
                    GetFirstPassStats(data.timestamp)[Stats.EnergyRecharge] * 0.3) 
                // TODO Will become part of burst MVs
            );
        }
        

        /*public override Dictionary<string, Func<double, List<WorldEvent>>> GetCharacterEvents()
        {
            return new Dictionary<string, Func<double, List<WorldEvent>>>
            {
                { "Skill", Skill }
            };
        }*/
    }
}