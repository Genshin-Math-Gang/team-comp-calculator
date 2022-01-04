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
        private readonly HitType GuobaHitType;
        private readonly HitType BurstInitialType;

        public Xiangling(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("xiangling", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.PYRO, WeaponType.POLEARM)
        {
            skillSnapshot = new SnapshottedStats(this, Types.SKILL);
            burstSnapshot = new SnapshottedStats(this, Types.BURST);
            // TODO: kqm didn't include frames for xl CA but included n1c so i guessed, needs to be checked
            AutoAttackFrameData = new[] {12, 38, 72, 141, 167, 78};
            BurstInitialICD = new();
            GuobaHitType = new HitType(Element.PYRO);
            BurstInitialType = new HitType(Element.PYRO, icd: BurstInitialICD);
            // TODO: make one for burst proper later
        }
        
        // TODO: the timing for this is sus

        public override List<WorldEvent> Skill(double timestamp, params object[] p)
        {
            return new List<WorldEvent> {
                SkillActivated(timestamp), // Order important: Guoba snapshots CW skill activation buff
                skillSnapshot.Snapshot(timestamp),
                new Hit(timestamp, 0, skillSnapshot.GetStats, this, Types.SKILL,
                    GuobaHitType, "guoba"),
                new Hit(timestamp + 3.5, 0, skillSnapshot.GetStats, this, Types.SKILL,
                    GuobaHitType, "guoba"),
                new Hit(timestamp + 5, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                    GuobaHitType, "guoba"),
                new Hit(timestamp + 7.5, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                    GuobaHitType, "guoba"),
            };
        }

        public override List<WorldEvent> Burst(double timestamp)
        {
            return new List<WorldEvent> {
                BurstActivated(timestamp),
                new Hit(timestamp, 0, GetStatsPage, this, Types.BURST, 
                    BurstInitialType, "nado initial 1st hit"),
                new Hit(timestamp + 0.5, 1, GetStatsPage, this, Types.BURST, 
                    BurstInitialType, "nado initial 2nd hit"),
                new Hit(timestamp + 1, 2, GetStatsPage, this, Types.BURST, 
                    BurstInitialType, "nado initial 3rd hit"),
                burstSnapshot.Snapshot(timestamp + 1)
            };
        }

        public List<WorldEvent> BurstHit(double timestamp)
        {
            return new List<WorldEvent> { new Hit(timestamp + 1, 3, burstSnapshot.GetStats, 
                this, Types.BURST, new HitType(Element.PYRO), "nado") };
        }

        

        /*public override Dictionary<string, Func<double, List<WorldEvent>>> GetCharacterEvents()
        {
            return new Dictionary<string, Func<double, List<WorldEvent>>>
            {
                { "Initial Burst", InitialBurst },
                { "Burst Hit", BurstHit },
                { "Skill", Skill }
            };
        }*/
    }
}