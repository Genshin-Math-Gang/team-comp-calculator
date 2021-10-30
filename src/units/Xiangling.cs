using System;
using System.Collections.Generic;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Weapons;

namespace Tcc.Units
{
    public class Xiangling: Unit
    {
        SnapshottedStats skillSnapshot, burstSnapshot;
        private static readonly ICDCreator BurstInitialICD = new("e225e4b3-54e0-450e-9145-6c7e175d8f31");

        public Xiangling(int constellationLevel): base(
            constellationLevel: constellationLevel,
            element: Element.PYRO,
            weaponType: WeaponType.POLEARM,
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
            burst: new AbilityStats(motionValues: new double[] {1.296,1.584,1.9728,2.016}, icd: new Timestamp(0))
        ) {
            this.skillSnapshot = new SnapshottedStats(this, Types.SKILL);
            this.burstSnapshot = new SnapshottedStats(this, Types.BURST);
        }
        
        // TODO: the timing for this is sus

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            return new List<WorldEvent> {
                SkillActivated(timestamp), // Order important: Guoba snapshots CW skill activation buff
                skillSnapshot.Snapshot(timestamp),
                new Hit(timestamp, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.SKILL,
                    new HitType(true, 1, false, false), "guoba"),
                new Hit(timestamp + 3.5, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.SKILL,
                    new HitType(true, 1, false, false), "guoba"),
                new Hit(timestamp + 5, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                    new HitType(true, 1, false, false), "guoba"),
                new Hit(timestamp + 7.5, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                    new HitType(true, 1, false, false), "guoba"),
            };
        }

        public List<WorldEvent> InitialBurst(Timestamp timestamp)
        {
            return new List<WorldEvent> {
                BurstActivated(timestamp),
                new Hit(timestamp, Element.PYRO, 0, GetStatsPage, this, Types.BURST, 
                    new HitType(false, 1, false, icd: BurstInitialICD), "nado initial 1st hit"),
                new Hit(timestamp + 0.5, Element.PYRO, 1, GetStatsPage, this, Types.BURST, 
                    new HitType(false, 1, false, icd: BurstInitialICD), "nado initial 2nd hit"),
                new Hit(timestamp + 1, Element.PYRO, 2, GetStatsPage, this, Types.BURST, 
                    new HitType(false, 1, false, icd: BurstInitialICD), "nado initial 3rd hit"),
                burstSnapshot.Snapshot(timestamp + 1)
            };
        }

        public List<WorldEvent> BurstHit(Timestamp timestamp)
        {
            return new List<WorldEvent> { new Hit(timestamp + 1, Element.PYRO, 3, burstSnapshot.GetStats, 
                this, Types.BURST, new HitType(false, 1, false, false), "nado") };
        }

        public override string ToString()
        {
            return "Xiangling";
        }

        /*public override Dictionary<string, Func<Timestamp, List<WorldEvent>>> GetCharacterEvents()
        {
            return new Dictionary<string, Func<Timestamp, List<WorldEvent>>>
            {
                { "Initial Burst", InitialBurst },
                { "Burst Hit", BurstHit },
                { "Skill", Skill }
            };
        }*/
    }
}