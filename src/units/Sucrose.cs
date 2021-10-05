using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Weapons;

namespace Tcc.Units
{
    public class Sucrose: Anemo
    {
        
        SnapshottedStats burstSnapshot;
        private static readonly ICDCreator NormalICD = new ("39dc65e1-8235-4c67-ac5c-591a19d87131");
        private static readonly ICDCreator BurstICD = new (new Timestamp(0), 0,"67a891f4-8cc8-4684-b627-cff7f11c6fcf");
        private const int UltInterval = 2;
        
        public Sucrose(int constellationLevel, int level=90) : base(
            constellationLevel: constellationLevel,
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
                critDamage: 0.5,
                level: level
            ),
            normal: new AbilityStats(motionValues: new double[] {0.6024,0.5511,0.6921,0.8625}),
            charged: new AbilityStats(motionValues: new double[] {2.1629}),
            plunge: new AbilityStats(motionValues: new double[] {1.1234,2.2462,2.8057}),
            skill: new AbilityStats(motionValues: new double[] {3.8016}),
            burst: new AbilityStats(motionValues: new double[] {2.6624, 0.792})
        )
        {
            burstSnapshot = new SnapshottedStats(this, Types.BURST);
        }



        // does there need to be normal attack hook
        public List<WorldEvent> NormalAttack(Timestamp timestamp, params object[] param)
        {
            var normalIndex = (int) param[0];
            return new List<WorldEvent>()
            {
                // i think sucrose autos have aoe, will check later
                new Hit(timestamp, element, normalIndex, GetStatsPage, this, Types.NORMAL, 
                    new HitType(false, 1, false, icd: NormalICD), $"Wind Spirit Creation {normalIndex}"),

            };
        }
        
        public List<WorldEvent> ChargedAttack(Timestamp timestamp, params object[] param)
        {
            var normalIndex = (int) param[0];
            return new List<WorldEvent>()
            {
                // i think sucrose autos have aoe, will check later
                new Hit(timestamp, element, normalIndex, GetStatsPage, this, Types.NORMAL, 
                    new HitType(true, 1, false, false), "Wind Spirit Creation CA" ),

            };
        }

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            
            return new List<WorldEvent>()
            {
                SkillActivated(timestamp),
                new Hit(timestamp, element, 0, GetStatsPage, this, Types.SKILL, 
                    new HitType(true, 1, false), "Astable Anemohypostasis Creation-6308" ),

            };
        }
        
        
        // TODO: when does sucrose burst snapshot
        // TODO: how does infusion work
        public List<WorldEvent> Burst(Timestamp timestamp)
        {
            Timestamp castingTime = new Timestamp(67.0 / 60);
            Timestamp endTime = timestamp + castingTime + 3 * UltInterval + (constellationLevel >= 2 ? 2: 0);
            Timestamp firstHit = timestamp + castingTime;
            var events = new List<WorldEvent>()
            {
                BurstActivated(timestamp),
                burstSnapshot.Snapshot(timestamp), 
                new AbilityInfusion(firstHit - .1, endTime, this),
                new Hit(firstHit, element, 0, burstSnapshot.GetStats, this, Types.BURST,
                    new HitType(true, 1, false, icd: BurstICD), "Forbidden Creation-Isomer 75/Type II"),
                new ConditionalHit(firstHit, GetInfusion, 1, burstSnapshot.GetStats, this, Types.BURST,
                    new HitType(true, 1, false, icd: BurstICD), DoInfusionHit,new object[] {this},"Forbidden Creation-Isomer 75/Type II infusion"),
                new AbilityInfusion(firstHit + UltInterval - .1, endTime, this),
                new Hit(firstHit + UltInterval, element, 0, burstSnapshot.GetStats, this, Types.BURST,
                    new HitType(true, 1, false, icd: BurstICD), "Forbidden Creation-Isomer 75/Type II"),
                new ConditionalHit(firstHit + UltInterval, GetInfusion, 1, burstSnapshot.GetStats, this, Types.BURST,
                    new HitType(true, 1, false, icd: BurstICD), DoInfusionHit,new object[] {this},"Forbidden Creation-Isomer 75/Type II infusion"),
                new AbilityInfusion(firstHit + 2 * UltInterval - .1, endTime, this),
                new Hit(firstHit + 2*UltInterval, element, 0, burstSnapshot.GetStats, this, Types.BURST,
                    new HitType(true, 1, false, icd: BurstICD), "Forbidden Creation-Isomer 75/Type II"),
                new ConditionalHit(firstHit + 2*UltInterval, GetInfusion, 1, burstSnapshot.GetStats, this, Types.BURST,
                    new HitType(true, 1, false, icd: BurstICD), DoInfusionHit,new object[] {this},"Forbidden Creation-Isomer 75/Type II infusion"),
                
            };

            if (constellationLevel < 2) return events;
            
            events.Add(new AbilityInfusion(firstHit + 3 * UltInterval - .1, endTime, this));
            events.Add(new Hit(firstHit + 3*UltInterval, element, 0, burstSnapshot.GetStats, this, Types.BURST,
                new HitType(true, 1, false, icd: BurstICD), "Forbidden Creation-Isomer 75/Type II"));
            events.Add(new ConditionalHit(firstHit + 3*UltInterval, GetInfusion, 1, burstSnapshot.GetStats, this, Types.BURST,
                new HitType(true, 1, false, icd: BurstICD), DoInfusionHit,new object[] {this},"Forbidden Creation-Isomer 75/Type II infusion"));

            return events;
        }
        
        private static bool DoInfusionHit(object[] param)
        {
            var unit = (Sucrose) param[0];
            return unit.Infusion != Element.PHYSICAL;
        }
        public override string ToString()
        {
            return "Sucrose";
        }
    }
    
    
}