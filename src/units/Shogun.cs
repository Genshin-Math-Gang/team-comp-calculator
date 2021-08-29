using System;
using System.Collections.Generic;
using Tcc.Buffs;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Shogun: Unit
    {
        static readonly Guid SKILL_BUFF_ID = new Guid("0e88cab3-e1d1-4592-9059-b36e6595e25d");
        static readonly Timestamp BUFF_DURATION = new Timestamp(25);

        EventHandler<(Unit from, Unit to, Timestamp)> currentBuffListener = null;

        public Shogun(int constellationLevel): base(
            constellationLevel: constellationLevel,
            element: Element.ELECTRO,
            burstEnergyCost: 90,
            capacityStats: new CapacityStats(
                baseHp: 10876,
                energy: 90
            ),
            generalStats: new GeneralStats(
                baseAttack: 225,
                attackPercent: 0.466,
                flatAttack: 311,
                critRate: 0.5,
                critDamage: 1
            ),
            normal: new AbilityStats(motionValues: new double[] {0.8806,0.8449,1.0795,1.11798,1.3209}),
            charged: new AbilityStats(motionValues: new double[] {1.105+1.202}),
            plunge: new AbilityStats(motionValues: new double[] {1.2638,2.527,3.1564}),
            skill: new AbilityStats(motionValues: new double[] {2.1096,0.756,0.03}),
            burst: new AbilityStats(motionValues: new double[] {4.1904,0.108,1.008})
        ) {
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
            events.Add(new Hit(timestamp, Element.ELECTRO, 0, GetStatsPage, this, Types.SKILL, false, true, true, 1, "shogun E"));

            return events;
        }

        Buff<AbilityModifier> CreateSkillBuff(Timestamp expiryTime)
        {
            return new RefreshableBuff<AbilityModifier>(
                SKILL_BUFF_ID,
                expiryTime: expiryTime,
                (data) => new GeneralStats(damagePercent: GetFirstPassStats(data.timestamp).EnergyRecharge * 0.3) // TODO Will become part of burst MVs
            );
        }

        public override string ToString()
        {
            return "Shogun";
        }

        public override Dictionary<string, Func<Timestamp, List<WorldEvent>>> GetCharacterEvents()
        {
            var dict = new Dictionary<string, Func<Timestamp, List<WorldEvent>>>();
            dict.Add("Skill", Skill);

            return dict;
        }
    }
}