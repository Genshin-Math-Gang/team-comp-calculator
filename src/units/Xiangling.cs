using System.Collections.Generic;
using Tcc.Events;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Xiangling: Unit
    {
        public Xiangling(int constellationLevel): base(
            constellationLevel,
            stats: new Stats.Stats(
                base_hp: 10876,
                base_attack: 225,
                attack_p: 0.466,
                attack_f: 311,
                crit_rate: 0.5,
                crit_dmg: 1
            ),
            normal: new Stats.Stats(new double[] {0.8313,0.833,0.5151*2,0.2788*4,1.4062}),
            charged: new Stats.Stats(new double[] {2.4055}),
            plunge: new Stats.Stats(new double[] {1.2638,2.527,3.1564}),
            skill: new Stats.Stats(new double[] {2.003}),
            burst: new Stats.Stats(new double[] {1.296,1.584,1.9728,2.016})
        ) {
        }

        public List<WorldEvent> InitialBurst(double timestamp) {
            return new List<WorldEvent> {
            new Snapshot(timestamp, this, Types.BURST, "nado"),
            new Hit(timestamp, () => getStats(Types.BURST), 0, this,"nado initial 1st hit"),
            new Hit(timestamp + 0.5, () => getStats(Types.BURST), 1, this, "nado initial 2nd hit"),
            new Hit(timestamp + 1.0, () => getStats(Types.BURST), 2, this, "nado initial 3rd hit"),
            new UnSnapshot(timestamp + 10, this, Types.BURST, "nado")};
        }

        public List<WorldEvent> Skill(double timestamp) {
            return new List<WorldEvent> {
            new Snapshot(timestamp, this, Types.SKILL, "Guoba"),
            new Hit(timestamp + 2.5, () => getStats(Types.SKILL), 0, this, "Guoba"),
            new Hit(timestamp + 5, () => getStats(Types.SKILL), 0, this, "Guoba"),
            new Hit(timestamp + 7.5, () => getStats(Types.SKILL), 0, this, "Guoba"),
            new Hit (timestamp + 10, () => getStats(Types.SKILL), 0, this, "Guoba"),
            new UnSnapshot(timestamp + 10, this, Types.SKILL, "Guoba")};
        }

        public List<WorldEvent> BurstHit(double timestamp) { 
            return new List<WorldEvent> {
            new Hit(timestamp, () => getStats(Types.BURST), 3, this, "nado spin")};
        }

        public override string ToString()
        {
            return "Xiangling";
        }
    }
}