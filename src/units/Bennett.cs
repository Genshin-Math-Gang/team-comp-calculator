using System;
using System.Collections.Generic;
using Tcc.buffs;
using Tcc.elements;
using Tcc.events;
using Tcc.stats;
using Tcc.weapons;

namespace Tcc.units
{
    public class Bennett: Unit
    {
        static readonly Guid BURST_BUFF_ID = new Guid("c1a23bde-db12-4589-9baf-d25b76ccb989");
        const int N_BURST_TICKS = 12;
        static readonly Timestamp BUFF_FREQUENCY = new Timestamp(1);
        static readonly Timestamp BUFF_DURATION = new Timestamp(2);
        private readonly HitType BurstHitType;

        SnapshottedStats burstBuffSnapshot;


        // TODO: add logic for cons increasing talent levels later
        public Bennett(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("bennett", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.PYRO, WeaponType.SWORD)
           

        {
            burstBuffSnapshot = new SnapshottedStats(this, Types.BURST);
            AutoAttackFrameData = new[] {12, 32, 63, 118, 167, 100};
            BurstHitType = new HitType(Element, gauge: 2);
        }
        
        
        public List<WorldEvent> Burst(Timestamp timestamp)
        {
            var events = new List<WorldEvent> {
                BurstActivated(timestamp),
                burstBuffSnapshot.Snapshot(timestamp),
            };

            for(int tick = 0; tick < N_BURST_TICKS; tick++)
            {
                var startTime = timestamp + tick * BUFF_FREQUENCY;

                // TODO Self-apply pyro
                // TODO make this use buff world event
                events.Add(new WorldEvent(startTime, 
                    (world) => world.OnFieldUnit.AddBuff(CreateBurstBuff(startTime)), "Bennett buff refresh", 1));

                // Deal burst damage after modifier snapshot and first application,
                if(tick == 0) events.Add(new Hit(timestamp, 0, GetStatsPage, this, 
                    Types.BURST, BurstHitType, "Bennett Burst"));
            }

            return events;
        }

        Buff<SecondPassModifier> CreateBurstBuff(Timestamp startTime)
        {
            var stats = burstBuffSnapshot.GetStats(startTime);
            // TODO: BENNET BURST MODIFIER IS WRONG
            var modifier = new StatsPage(Stats.AtkFlat, 
                stats[Stats.AtkBase] /** StartingAbilityStats[Types.BURST].GetMotionValue(2)*/);

            return new RefreshableBuff<SecondPassModifier>(BURST_BUFF_ID, startTime + BUFF_DURATION, (_) => modifier);
        }
        

        /*public override Dictionary<string, Func<Timestamp, List<WorldEvent>>> GetCharacterEvents()
        {
            return new Dictionary<string, Func<Timestamp, List<WorldEvent>>>
            {
                { "Burst", Burst }
            };
        }*/
    }
}