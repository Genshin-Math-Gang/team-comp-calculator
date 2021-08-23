using System;
using System.Collections.Generic;
using Tcc.Buffs.Characters;
using Tcc.Buffs;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Shogun: Unit
    {
        readonly static Guid SKILL_BUFF_ID = new Guid("0e88cab3-e1d1-4592-9059-b36e6595e25d");

        EventHandler<(Unit from, Unit to, Timestamp)> currentBuffListener = null;

        public Shogun(int constellationLevel): base(
            constellationLevel,
            stats: new Stats.Stats(
                baseHp: 10876,
                baseAttack: 225,
                attackPercent: 0.466,
                flatAttack: 311,
                critRate: 0.5,
                critDamage: 1,
                energyRecharge: 2.5
            ),
            normal: new Stats.Stats(new double[] {0.8806,0.8449,1.0795,1.11798,1.3209}),
            charged: new Stats.Stats(new double[] {1.105+1.202}),
            plunge: new Stats.Stats(new double[] {1.2638,2.527,3.1564}),
            skill: new Stats.Stats(new double[] {2.4768,1.512+1.656,1.584+1.728,2.376}),
            burst: new Stats.Stats(new double[] {4.1904,0.108,1.008})
        ) {
        }

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            var events = new List<WorldEvent>();
            var expiryTime = timestamp + 25;

            EventHandler<(Unit from, Unit to, Timestamp)> newBuffListener = null;

            events.Add(new WorldEvent(timestamp, (world) =>
            {
                Console.WriteLine($"Shogun skill at {timestamp}");
                world.unitSwapped -= currentBuffListener;

                this.RemoveAllBuff(SKILL_BUFF_ID);
                this.AddBuff(CreateSkillBuff(world, expiryTime));

                newBuffListener = (_, data) =>
                {
                    data.from.RemoveAllBuff(SKILL_BUFF_ID);
                    data.to.AddBuff(CreateSkillBuff(world, expiryTime));
                };

                world.unitSwapped += newBuffListener;
                currentBuffListener = newBuffListener;
            }));

            events.Add(new WorldEvent(expiryTime, (world) => world.unitSwapped -= newBuffListener));

            return events;
        }

        BuffFromStats CreateSkillBuff(World world, Timestamp expiryTime)
        {
            return new BasicBuffFromStats(
                SKILL_BUFF_ID,
                (_, _, timestamp) => new Stats.Stats(damagePercent: this.GetStatsFromUnitWithoutScaled(Types.BURST, timestamp).EnergyRecharge * 0.3),
                Types.BURST,
                expiryTime: expiryTime
            );
        }

        public override string ToString()
        {
            return "Shogun";
        }
    }
}