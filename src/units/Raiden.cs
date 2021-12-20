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
        static readonly Timestamp BUFF_DURATION = new Timestamp(25);
        // raiden will need more ICD stuff but i need to find that

        EventHandler<(Unit from, Unit to, Timestamp)> currentBuffListener = null;

        public Raiden(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("raiden", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.ELECTRO, WeaponType.POLEARM) 
        {
            AutoAttackFrameData = new[] {14, 31, 56, 102, 151, 172, 44};
            int[] AutoAttackBurstFrameData = new[] {12, 32, 54, 95, 139, 215, 50};
            SkillICD = new();
        }

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            var events = new List<WorldEvent>();
            var expiryTime = timestamp + BUFF_DURATION; //Bugged because it was static

            EventHandler<(Unit from, Unit to, Timestamp)> newBuffListener = null;

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
            events.Add(new Hit(timestamp, Element.ELECTRO, 0, GetStatsPage, this, Types.SKILL, 
                new HitType(true, 1, false, icd: SkillICD)));

            return events;
        }

        private Buff<AbilityModifier> CreateSkillBuff(Timestamp expiryTime)
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

        public override string ToString()
        {
            return "Shogun";
        }

        /*public override Dictionary<string, Func<Timestamp, List<WorldEvent>>> GetCharacterEvents()
        {
            return new Dictionary<string, Func<Timestamp, List<WorldEvent>>>
            {
                { "Skill", Skill }
            };
        }*/
    }
}