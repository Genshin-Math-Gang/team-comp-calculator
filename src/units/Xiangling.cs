using System.Collections.Generic;
using Tcc.elements;
using Tcc.events;
using Tcc.stats;
using Tcc.weapons;

namespace Tcc.units
{
    public class Xiangling: Unit
    {
        SnapshottedStats skillSnapshot, burstSnapshot;
        private static readonly ICDCreator BurstInitialICD = new("e225e4b3-54e0-450e-9145-6c7e175d8f31");

        public Xiangling(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("xiangling", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.PYRO, WeaponType.POLEARM)
        {
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
                    new HitType(true, 1, false, icd: BurstInitialICD), "nado initial 1st hit"),
                new Hit(timestamp + 0.5, Element.PYRO, 1, GetStatsPage, this, Types.BURST, 
                    new HitType(true, 1, false, icd: BurstInitialICD), "nado initial 2nd hit"),
                new Hit(timestamp + 1, Element.PYRO, 2, GetStatsPage, this, Types.BURST, 
                    new HitType(true, 1, false, icd: BurstInitialICD), "nado initial 3rd hit"),
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