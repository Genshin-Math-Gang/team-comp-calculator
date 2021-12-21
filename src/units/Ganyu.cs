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
        private readonly HitType IcicleHitType;
        private readonly HitType LotusHitType;
        

        public Ganyu(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("ganyu", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.CRYO, WeaponType.BOW)
        {
            skillSnapshot = new SnapshottedStats(this, Types.SKILL);
            burstSnapshot = new SnapshottedStats(this, Types.BURST);
            AutoAttackFrameData = new[] {18, 43, 73, 117, 153, 190, 94, 115};
            ICDCreator BurstICD = new ICDCreator();
            IcicleHitType = new HitType(Element.CRYO, icd: BurstICD);
            LotusHitType = new HitType(Element.CRYO);
        }

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            return new List<WorldEvent>()
            {
                SkillActivated(timestamp),
                skillSnapshot.Snapshot(timestamp),
                new Hit(timestamp, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                 LotusHitType, "Trail of the Qilin cast" ),
                new Hit(timestamp + 6, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                    LotusHitType,  "Trail of the Qilin explosion" )

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
                    events.Add(new Hit(time, 0, burstSnapshot.GetStats, this, Types.BURST,
                       IcicleHitType,"Icicle hit"));
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
            return new Hit(timestamp, 0, burstSnapshot.GetStats, this, Types.BURST,
               IcicleHitType,"Icicle hit");
        }

        // TODO: rewrite this to be up to standard later
        public List<WorldEvent> ChargedAttack(Timestamp timestamp, params object[] param)
        {
            int chargeLevel = (int) param[1];
            // TODO: what is icd override and how do i use it
            return chargeLevel switch
            {
                1 => new List<WorldEvent>(){new Hit(timestamp, 0,GetStatsPage, this, 
                    Types.CHARGED, new HitType(Element.PHYSICAL, false), "Charged Attack")},
                2 => new List<WorldEvent>(){new Hit(timestamp, 1,GetStatsPage, this, 
                    Types.CHARGED, new HitType(Element.CRYO, false), "Fully Aimed charged shot")},
                3 => new List<WorldEvent>(){new Hit(timestamp, 2,GetStatsPage, this, 
                    Types.CHARGED, new HitType(Element.CRYO,false), "Frostlake Arrow"),
                    new Hit(timestamp+.3, 3,GetStatsPage, this, 
                        Types.CHARGED, new HitType(Element.CRYO), "Frostlake Arrow bloom")
                }
            };
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