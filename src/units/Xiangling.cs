using System.Collections.Generic;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Xiangling: Unit
    {
        SnapshottedStats skillSnapshot, burstSnapshot;

        public Xiangling(int constellationLevel): base(
            constellationLevel,
            element: Element.PYRO,
            stats: new Stats.Stats(
                baseHp: 10876,
                baseAttack: 225,
                attackPercent: 0.466,
                flatAttack: 311,
                critRate: 0.5,
                critDamage: 1
            ),
            normal: new Stats.Stats(new double[] {0.8313,0.833,0.5151*2,0.2788*4,1.4062}),
            charged: new Stats.Stats(new double[] {2.4055}),
            plunge: new Stats.Stats(new double[] {1.2638,2.527,3.1564}),
            skill: new Stats.Stats(new double[] {2.003}),
            burst: new Stats.Stats(new double[] {1.296,1.584,1.9728,2.016})
        ) {
            this.skillSnapshot = new SnapshottedStats(this, Types.SKILL);
            this.burstSnapshot = new SnapshottedStats(this, Types.BURST);
        }

        public List<WorldEvent> InitialBurst(Timestamp timestamp)
        {
            return new List<WorldEvent> {
                BurstActivated(timestamp),
                new Hit(timestamp, Element.PYRO, GetStats(Types.BURST), 0, this,"nado initial 1st hit"),
                new Hit(timestamp + 0.5, Element.PYRO, GetStats(Types.BURST), 1, this, "nado initial 2nd hit"),
                new Hit(timestamp + 1, Element.PYRO, GetStats(Types.BURST), 2, this, "nado initial 3rd hit"),
                burstSnapshot.Snapshot(timestamp + 1)
            };
        }

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            return new List<WorldEvent> {
                SkillActivated(timestamp), // Order important: Guoba snapshots CW skill activation buff
                skillSnapshot.Snapshot(timestamp),
                new Hit(timestamp + 2.5, Element.PYRO, skillSnapshot.GetStats, 0, this, "Guoba"),
                new Hit(timestamp + 5, Element.PYRO, skillSnapshot.GetStats, 0, this, "Guoba"),
                new Hit(timestamp + 7.5, Element.PYRO, skillSnapshot.GetStats, 0, this, "Guoba"),
                new Hit(timestamp + 10, Element.PYRO, skillSnapshot.GetStats, 0, this, "Guoba")
            };
        }

        public List<WorldEvent> BurstHit(Timestamp timestamp)
        {
            return new List<WorldEvent> { new Hit(timestamp, Element.PYRO, burstSnapshot.GetStats, 3, this, "nado spin") };
        }

        public override string ToString()
        {
            return "Xiangling";
        }
    }
}