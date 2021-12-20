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
        private ICDCreator BurstInitialICD;

        public Xiangling(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("xiangling", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.PYRO, WeaponType.POLEARM)
        {
            skillSnapshot = new SnapshottedStats(this, Types.SKILL);
            burstSnapshot = new SnapshottedStats(this, Types.BURST);
            // TODO: kqm didn't include frames for xl CA but included n1c so i guessed, needs to be checked
            AutoAttackFrameData = new[] {12, 38, 72, 141, 167, 78};
            BurstInitialICD = new();
        }
        
        // TODO: the timing for this is sus

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            return new List<WorldEvent> {
                SkillActivated(timestamp), // Order important: Guoba snapshots CW skill activation buff
                skillSnapshot.Snapshot(timestamp),
                new Hit(timestamp, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.SKILL,
                    new HitType(true), "guoba"),
                new Hit(timestamp + 3.5, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.SKILL,
                    new HitType(true), "guoba"),
                new Hit(timestamp + 5, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                    new HitType(true), "guoba"),
                new Hit(timestamp + 7.5, Element.PYRO, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                    new HitType(true), "guoba"),
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