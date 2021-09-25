using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Weapons;

namespace Tcc.Units
{
    public class Sucrose: Unit
    {
        
        SnapshottedStats burstSnapshot;
        private static readonly ICDCreator NormalICD = new ICDCreator("39dc65e1-8235-4c67-ac5c-591a19d87131");
        
        public Sucrose(int constellationLevel) : base(
            constellationLevel: constellationLevel,
            element: Element.ANEMO,
            weaponType: WeaponType.CATALYST,
            burstEnergyCost: 80,
            capacityStats: new CapacityStats(
                baseHp: 9244,
                energy: 80
            ),
            generalStats: new GeneralStats(
                baseAttack: 170,
                attackPercent: 0.466,
                flatAttack: 311,
                critRate: 0.05,
                critDamage: 0.5
            ),
            normal: new AbilityStats(motionValues: new double[] {0.6024,0.5511,0.6921,0.8625}),
            charged: new AbilityStats(motionValues: new double[] {2.1629}),
            plunge: new AbilityStats(motionValues: new double[] {1.1234,2.2462,2.8057}),
            skill: new AbilityStats(motionValues: new double[] {3.8016}),
            burst: new AbilityStats(motionValues: new double[] {2.6624, 0.792})
        )
        {
            this.burstSnapshot = new SnapshottedStats(this, Types.BURST);
        }



        // does there need to be normal attack hook
        public List<WorldEvent> NormalAttack(Timestamp timestamp, params object[] param)
        {
            var normalIndex = (int) param[0];
            return new List<WorldEvent>()
            {
                // i think sucrose autos have aoe, will check later
                new Hit(timestamp, element, normalIndex, GetStatsPage, this, Types.NORMAL, 
                    isAoe: true, description: $"Wind Spirit Creation {normalIndex}", creator: NormalICD),

            };
        }
        
        public List<WorldEvent> ChargedAttack(Timestamp timestamp, params object[] param)
        {
            var normalIndex = (int) param[0];
            return new List<WorldEvent>()
            {
                // i think sucrose autos have aoe, will check later
                new Hit(timestamp, element, normalIndex, GetStatsPage, this, Types.NORMAL, 
                    isAoe: true, description: "Wind Spirit Creation CA" ),

            };
        }

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            return new List<WorldEvent>()
            {
                SkillActivated(timestamp),
                new Hit(timestamp, element, 0, GetStatsPage, this, Types.SKILL, 
                    isAoe: true, description: "Astable Anemohypostasis Creation-6308" ),

            };
        }
        
        
        // TODO: when does sucrose burst snapshot
        // TODO: how does infusion work
        public List<WorldEvent> Burst(Timestamp timestamp)
        {
            var events = new List<WorldEvent>()
            {
                BurstActivated(timestamp),
                new Hit(timestamp + 1 + 7 / 60, element, 0, burstSnapshot.GetStats, this, Types.BURST,
                    isAoe: true, description: "Forbidden Creation-Isomer 75/Type II"),
                new Hit(timestamp + 3 +7 / 60, element, 0, burstSnapshot.GetStats, this, Types.BURST,
                    isAoe: true, description: "Forbidden Creation-Isomer 75/Type II"),
                new Hit(timestamp + 5 + 7 / 60, element, 0, burstSnapshot.GetStats, this, Types.BURST,
                    isAoe: true, description: "Forbidden Creation-Isomer 75/Type II")
            };
            if (constellationLevel >= 2)
            {
                events.Add(new Hit(timestamp + 7 + 7 / 60, element, 0, burstSnapshot.GetStats, this, Types.BURST,
                    isAoe: true, description: "Forbidden Creation-Isomer 75/Type II"));
            }

            return events;
        }
        
        public override string ToString()
        {
            return "Sucrose";
        }
    }
    
    
}