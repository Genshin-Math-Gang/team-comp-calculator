using System;
using System.Collections.Generic;
using Tcc.elements;
using Tcc.events;
using Tcc.stats;
using Tcc.weapons;

namespace Tcc.units
{
    public class Ganyu: Unit
    {
        SnapshottedStats skillSnapshot, burstSnapshot;
        private static readonly ICDCreator BurstICD = new ICDCreator("c6de4351-963c-4775-bbe8-4b0b38bc5a9d");

        public Ganyu(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("ganyu", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.CRYO, WeaponType.BOW)
        {
            skillSnapshot = new SnapshottedStats(this, Types.SKILL);
            burstSnapshot = new SnapshottedStats(this, Types.BURST);
            AutoAttackFrameData = new[] {18, 43, 73, 117, 153, 190, 94, 115};
        }

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            return new List<WorldEvent>()
            {
                SkillActivated(timestamp),
                skillSnapshot.Snapshot(timestamp),
                new Hit(timestamp, Element, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                   new HitType(true, 1, false, false), "Trail of the Qilin cast" ),
                new Hit(timestamp + 6, Element, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                    new HitType(true, 1, false, false),  "Trail of the Qilin explosion" )

            };
        }

        public List<WorldEvent> BurstCast(Timestamp timestamp, params object[] param)
        {
            var world = (World) param[0];
            int num = Math.Max(world.Enemies.Count, 4);
            var events = new List<WorldEvent>()
            {
                BurstActivated(timestamp),
                burstSnapshot.Snapshot(timestamp)
            };
            for (int i = 0; i < 10; i++)
            {
                // need to adapt for number of enemies
                Timestamp time = timestamp + new Timestamp(1.5 * i);
                for (int j = 0; j < num; j++)
                {
                    time += .3;
                    events.Add(new Hit(time, Element, 0, burstSnapshot.GetStats, this, Types.BURST,
                        new HitType(true, 1, false, icd: BurstICD),"Icicle hit"));
                }
            }

            return events;
        }

        // kinda deprecated
        public List<WorldEvent> BurstIcicle(Timestamp timestamp)
        {
            return new List<WorldEvent>()
            {
               Icicle(timestamp)
            };
        }

        private WorldEvent Icicle(Timestamp timestamp)
        {
            return new Hit(timestamp, Element, 0, burstSnapshot.GetStats, this, Types.BURST,
                new HitType(true, 1, false, false),"Icicle hit");
        }

        public List<WorldEvent> ChargedAttack(Timestamp timestamp, params object[] param)
        {
            int chargeLevel = (int) param[1];
            // TODO: what is icd override and how do i use it
            return chargeLevel switch
            {
                1 => new List<WorldEvent>(){new Hit(timestamp, Element.PHYSICAL, 0,GetStatsPage, this, 
                    Types.CHARGED, new HitType(false, 1, false, false), "Charged Attack")},
                2 => new List<WorldEvent>(){new Hit(timestamp, Element.CRYO, 1,GetStatsPage, this, 
                    Types.CHARGED, new HitType(false, 1, false, false), "Fully Aimed charged shot")},
                3 => new List<WorldEvent>(){new Hit(timestamp, Element.CRYO, 2,GetStatsPage, this, 
                    Types.CHARGED, new HitType(false, 1, false, false), "Frostlake Arrow"),
                    new Hit(timestamp+.3, Element.CRYO, 3,GetStatsPage, this, 
                        Types.CHARGED, new HitType(true, 1, false, false), "Frostlake Arrow bloom")
                }
            };
        }

        public override string ToString()
        {
            return "Ganyu";
        }
        
        /*public override Dictionary<string,Delegate> GetCharacterEvents()
        {
            return new Dictionary<string, Delegate>
            {
                { "Cast", BurstCast },
                { "Icicle", BurstIcicle },
                { "Skill", Skill }
            };
        }*/
    }
}