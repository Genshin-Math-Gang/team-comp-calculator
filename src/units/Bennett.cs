using System;
using System.Collections.Generic;
using Tcc.Buffs;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Bennett: Unit
    {
        static readonly Guid BURST_BUFF_ID = new Guid("c1a23bde-db12-4589-9baf-d25b76ccb989");
        const int N_BURST_TICKS = 12;
        static readonly Timestamp BUFF_FREQUENCY = new Timestamp(1);
        static readonly Timestamp BUFF_DURATION = new Timestamp(2);

        SnapshottedStats burstBuffSnapshot;

        public Bennett(int constellationLevel): base(
            constellationLevel: constellationLevel,
            element: Element.PYRO,
            burstEnergyCost: 60,
            capacityStats: new CapacityStats(
                baseHp: 10876,
                energy: 60
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
            skill: new AbilityStats(motionValues: new double[] {2.4768,1.512+1.656,1.584+1.728,2.376}),
            burst: new AbilityStats(motionValues: new double[] {4.1904,0.108,1.008})
        ) {
            this.burstBuffSnapshot = new SnapshottedStats(this, Types.BURST);
        }

        public List<WorldEvent> Burst(Timestamp timestamp)
        {
            var events = new List<WorldEvent> {
                BurstActivated(timestamp),
                burstBuffSnapshot.Snapshot(timestamp),
            };

            for(int tick = 0; tick < N_BURST_TICKS; tick++)
            {
                var startTime = timestamp + tick * BUFF_FREQUENCY;

                // TODO Self-apply pyro
                events.Add(new WorldEvent(startTime, (world) => world.OnFieldUnit.AddBuff(CreateBurstBuff(startTime))));

                // Deal burst damage after modifier snapshot and first application,
                if(tick == 0) events.Add(new Hit(timestamp, Element.PYRO, 0, GetStatsPage, this, Types.BURST, false, true, 1, 1, "burst"));
            }

            return events;
        }

        Buff<SecondPassModifier> CreateBurstBuff(Timestamp startTime)
        {
            var stats = burstBuffSnapshot.GetStats(startTime);
            var modifier = new GeneralStats(flatAttack: stats.Attack.Base * startingAbilityStats[Types.BURST].GetMotionValue(2));

            return new RefreshableBuff<SecondPassModifier>(BURST_BUFF_ID, startTime + BUFF_DURATION, (_) => modifier);
        }

        public override string ToString()
        {
            return "Bennett";
        }

        public override Dictionary<string, Func<Timestamp, List<WorldEvent>>> GetCharacterEvents()
        {
            var dict = new Dictionary<string, Func<Timestamp, List<WorldEvent>>>();
            dict.Add("Burst", Burst);

            return dict;
        }
    }
}