using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Weapons;

namespace Tcc.Units
{
    public class Ganyu: Unit
    {
        SnapshottedStats skillSnapshot, burstSnapshot;
        private static readonly ICDCreator BurstICD = new ICDCreator("c6de4351-963c-4775-bbe8-4b0b38bc5a9d");

        public Ganyu(int constellationLevel) : base(
            constellationLevel: constellationLevel,
            element: Element.CRYO,
            weaponType: WeaponType.BOW,
            burstEnergyCost: 60,
            capacityStats: new CapacityStats(
                baseHp: 9797,
                energy: 60
            ),
            generalStats: new GeneralStats(
                // this is definitely sus right now, hopefully artifacts will be implemented soon
                baseAttack: 335,
                attackPercent: 0.466,
                flatAttack: 311,
                critRate: 0.05,
                critDamage: 0.884
            ),
            normal: new AbilityStats(motionValues: new double[] {0.6273,0.7038,0.8993,0.8993,0.9537,1.139}),
            charged: new AbilityStats(motionValues: new double[] {0.867,2.232,2.304,3.9168}),
            plunge: new AbilityStats(motionValues: new double[] {1.1234,2.2462,2.8057}),
            skill: new AbilityStats(motionValues: new double[] {2.376}),
            burst: new AbilityStats(motionValues: new double[] {1.2649}, icd: new Timestamp(0))
        )
        {
            this.skillSnapshot = new SnapshottedStats(this, Types.SKILL);
            this.burstSnapshot = new SnapshottedStats(this, Types.BURST);
        }

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            return new List<WorldEvent>()
            {
                SkillActivated(timestamp),
                skillSnapshot.Snapshot(timestamp),
                new Hit(timestamp, element, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                    isAoe: true, description: "Trail of the Qilin cast" ),
                new Hit(timestamp + 6, element, 0, skillSnapshot.GetStats, this, Types.SKILL, 
                    isAoe: true, description: "Trail of the Qilin explosion" )

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
                Timestamp time = timestamp + new Timestamp(1.5 * i);
                for (int j = 0; j < num; j++)
                {
                    time += .3;
                    events.Add(new Hit(time, element, 0, burstSnapshot.GetStats, this, Types.BURST,
                        isAoe: true,description: "Icicle hit", creator: BurstICD));
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
            return new Hit(timestamp, element, 0, burstSnapshot.GetStats, this, Types.BURST,
                isAoe: true,description: "Icicle hit");
        }

        public List<WorldEvent> ChargedAttack(Timestamp timestamp, params object[] param)
        {
            int chargeLevel = (int) param[1];
            // TODO: what is icd override and how do i use it
            return chargeLevel switch
            {
                1 => new List<WorldEvent>(){new Hit(timestamp, Element.PHYSICAL, 0,GetStatsPage, this, 
                    Types.CHARGED, isAoe: false, description: "Charged Attack")},
                2 => new List<WorldEvent>(){new Hit(timestamp, Element.CRYO, 1,GetStatsPage, this, 
                    Types.CHARGED, isAoe: false, description: "Fully Aimed charged shot")},
                3 => new List<WorldEvent>(){new Hit(timestamp, Element.CRYO, 2,GetStatsPage, this, 
                    Types.CHARGED, isAoe: false, description: "Frostlake Arrow"),
                    new Hit(timestamp+.3, Element.CRYO, 3,GetStatsPage, this, 
                        Types.CHARGED, isAoe: true, description: "Frostlake Arrow bloom")
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