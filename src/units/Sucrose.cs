using System;
using System.Collections.Generic;
using Tcc.elements;
using Tcc.events;
using Tcc.stats;
using Tcc.weapons;

namespace Tcc.units
{
    public class Sucrose: Anemo
    {
        
        SnapshottedStats burstSnapshot;
        private const int UltInterval = 2;
        private readonly HitType AnemoHitType;
        private readonly HitType ConditionalHitType;
        
        public Sucrose(int constellationLevel=0, String level="90" , int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("sucrose", level, constellationLevel, autoLevel, skillLevel, burstLevel, WeaponType.CATALYST)
        {
            burstSnapshot = new SnapshottedStats(this, Types.BURST);
            AutoAttackFrameData = new[] {19, 38, 70, 101, 132, 53};
            BurstICD = new ICDCreator(0,0);
            AnemoHitType = new HitType(Element.ANEMO);
            ConditionalHitType = new HitType(InfusionRef);
        }
        

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            
            return new List<WorldEvent>()
            {
                SkillActivated(timestamp),
                new Hit(timestamp, 0, GetStatsPage, this, Types.SKILL, 
                    AnemoHitType, "Astable Anemohypostasis Creation-6308" ),

            };
        }
        
        
        // TODO: when does sucrose burst snapshot
        // TODO: how does infusion work
        public List<WorldEvent> Burst(Timestamp timestamp)
        {
            Timestamp castingTime = new Timestamp(67.0 / 60);
            Timestamp endTime = timestamp + castingTime + 3 * UltInterval + (ConstellationLevel >= 2 ? 2: 0);
            Timestamp firstHit = timestamp + castingTime;
            var events = new List<WorldEvent>()
            {
                BurstActivated(timestamp),
                burstSnapshot.Snapshot(timestamp), 
                new AbilityInfusion(firstHit - .1, endTime, this),
                new Hit(firstHit, 0, burstSnapshot.GetStats, this, Types.BURST,
                    AnemoHitType, "Forbidden Creation-Isomer 75/Type II"),
                new ConditionalHit(firstHit, 1, burstSnapshot.GetStats, this, Types.BURST,
                    ConditionalHitType, DoInfusionHit,new object[] {this},"Forbidden Creation-Isomer 75/Type II infusion"),
                new AbilityInfusion(firstHit + UltInterval - .1, endTime, this),
                new Hit(firstHit + UltInterval, 0, burstSnapshot.GetStats, this, Types.BURST,
                    AnemoHitType, "Forbidden Creation-Isomer 75/Type II"),
                new ConditionalHit(firstHit + UltInterval, 1, burstSnapshot.GetStats, this, Types.BURST,
                    ConditionalHitType, DoInfusionHit,new object[] {this},"Forbidden Creation-Isomer 75/Type II infusion"),
                new AbilityInfusion(firstHit + 2 * UltInterval - .1, endTime, this),
                new Hit(firstHit + 2*UltInterval, 0, burstSnapshot.GetStats, this, Types.BURST,
                    AnemoHitType, "Forbidden Creation-Isomer 75/Type II"),
                new ConditionalHit(firstHit + 2*UltInterval, 1, burstSnapshot.GetStats, this, Types.BURST,
                    ConditionalHitType, DoInfusionHit,new object[] {this},"Forbidden Creation-Isomer 75/Type II infusion"),
                
            };

            if (ConstellationLevel < 2) return events;
            
            events.Add(new AbilityInfusion(firstHit + 3 * UltInterval - .1, endTime, this));
            events.Add(new Hit(firstHit + 3*UltInterval, 0, burstSnapshot.GetStats, this,
                Types.BURST, AnemoHitType, "Forbidden Creation-Isomer 75/Type II"));
            events.Add(new ConditionalHit(firstHit + 3*UltInterval, 1, burstSnapshot.GetStats, this, Types.BURST,
                ConditionalHitType, DoInfusionHit,new object[] {this},"Forbidden Creation-Isomer 75/Type II infusion"));

            return events;
        }
        
        private static bool DoInfusionHit(object[] param)
        {
            var unit = (Sucrose) param[0];
            return unit.Infusion != Element.PHYSICAL;
        }

    }
    
    
}