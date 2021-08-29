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
            constellationLevel: constellationLevel,
            element: Element.PYRO,
            burstEnergyCost: 80,
            capacityStats: new CapacityStats(
                baseHp: 10876,
                energy: 80
            ),
            generalStats: new GeneralStats(
                baseAttack: 225,
                attackPercent: 0.466,
                flatAttack: 311,
                critRate: 0.5,
                critDamage: 1
            ),
            normal: new AbilityStats(motionValues: new double[] {0.8313,0.833,0.5151*2,0.2788*4,1.4062}),
            charged: new AbilityStats(motionValues: new double[] {2.4055}),
            plunge: new AbilityStats(motionValues: new double[] {1.2638,2.527,3.1564}),
            skill: new AbilityStats(motionValues: new double[] {2.003}),
            burst: new AbilityStats(motionValues: new double[] {1.296,1.584,1.9728,2.016}, new Timestamp(0))
        ) {
            this.skillSnapshot = new SnapshottedStats(this, Types.SKILL);
            this.burstSnapshot = new SnapshottedStats(this, Types.BURST);
        }

        public List<WorldEvent> InitialBurst(Timestamp timestamp)
        {
            return new List<WorldEvent> {
                BurstActivated(timestamp),
                new Hit(timestamp, Element.PYRO, 0, GetStats(Types.BURST), this, Types.BURST, false, true, true, 1, "nado initial 1st hit"),
                new Hit(timestamp + 0.5, Element.PYRO, 1, GetStats(Types.BURST), this, Types.BURST, false, true, true, 1, "nado initial 2nd hit"),
                new Hit(timestamp + 1, Element.PYRO, 2, GetStats(Types.BURST), this, Types.BURST, false, true, true, 1, "nado initial 3rd hit"),
                burstSnapshot.Snapshot(timestamp + 1)
            };
        }

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            return new List<WorldEvent> {
                SkillActivated(timestamp), // Order important: Guoba snapshots CW skill activation buff
                skillSnapshot.Snapshot(timestamp),
                new Hit(timestamp + 2, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.BURST, false, true, true, 1, "Guoba"),
                new Hit(timestamp + 3.5, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.BURST, false, true, true, 1, "Guoba"),
                new Hit(timestamp + 5, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.BURST, false, true, true, 1, "Guoba"),
                new Hit(timestamp + 7.5, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.BURST, false, true, true, 1, "Guoba")
            };
        }

        public List<WorldEvent> BurstHit(Timestamp timestamp)
        {
            return new List<WorldEvent> { new Hit(timestamp, Element.PYRO, 3, burstSnapshot.GetStats, this, Types.BURST, false, true, true, 1, "nado spin") };
        }

        public override string ToString()
        {
            return "Xiangling";
        }
    }
}