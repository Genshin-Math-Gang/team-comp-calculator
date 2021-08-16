using System.Collections.Generic;
using Tcc.Buffs.Characters;
using Tcc.Events;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Bennett: Unit
    {
        const int N_BURST_TICKS = 12;

        static readonly Timestamp BUFF_FREQUENCY = new Timestamp(1);

        Stats.Stats burstBuffModifier;

        public Bennett(int constellationLevel): base(
            constellationLevel,
            stats: new Stats.Stats(
                base_hp: 10876,
                base_attack: 225,
                attack_p: 0.466,
                attack_f: 311,
                crit_rate: 0.5,
                crit_dmg: 1
            ),
            normal: new Stats.Stats(new double[] {0.8806,0.8449,1.0795,1.11798,1.3209}),
            charged: new Stats.Stats(new double[] {1.105+1.202}),
            plunge: new Stats.Stats(new double[] {1.2638,2.527,3.1564}),
            skill: new Stats.Stats(new double[] {2.4768,1.512+1.656,1.584+1.728,2.376}),
            burst: new Stats.Stats(new double[] {4.1904,0.108,1.008})
        ) {
        }

        public List<WorldEvent> Burst(Timestamp timestamp)
        {
            // FIXME retrieval by timestamp must happen in a worldevent, this will break
            var modifier = new Stats.Stats(attack_f: getStats(Types.BURST, timestamp).BaseAttack * getStats(Types.BURST, timestamp).MV[2]);

            var events = new List<WorldEvent>();

            for(int tick = 0; tick < N_BURST_TICKS; tick++)
            {
                var startTime = timestamp + tick * BUFF_FREQUENCY;
                events.Add(new AddBuffOnField(startTime, new BennettBurstBuff(modifier, startTime), "Bennett burst buff added to on-field unit"));
            }

            events.Insert(1, new Hit(timestamp, getStats(Types.BURST), 0, this, "Bennett burst"));

            return events;
        }

        public override string ToString()
        {
            return "Bennett";
        }
    }
}