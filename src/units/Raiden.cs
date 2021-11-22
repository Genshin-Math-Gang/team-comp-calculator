using System;
using System.Collections.Generic;
using Tcc.Buffs;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Weapons;

namespace Tcc.Units
{
    public class Raiden: Unit
    {
        static readonly Guid SKILL_BUFF_ID = new Guid("0e88cab3-e1d1-4592-9059-b36e6595e25d");
        static readonly Timestamp BUFF_DURATION = new Timestamp(25);
        // raiden will need more ICD stuff but i need to find that
        private static readonly ICDCreator SkillICD = new ICDCreator("4c77095b-9d2d-4e3d-8568-1697b60b7503");

        EventHandler<(Unit from, Unit to, Timestamp)> currentBuffListener = null;

        public Raiden(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("raiden", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.ELECTRO, WeaponType.POLEARM) 
        {
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

        Buff<AbilityModifier> CreateSkillBuff(Timestamp expiryTime)
        {
            return new RefreshableBuff<AbilityModifier>(
                SKILL_BUFF_ID,
                expiryTime: expiryTime,
                (data) => new GeneralStats(damagePercent: GetFirstPassStats(data.timestamp).EnergyRecharge * 0.3) 
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