using System;
using Tcc.elements;
using Tcc.enemy;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events
{
    public class Hit: WorldEvent 
        // TODO: it might just be better to replace mvIndex with the MV itself
    {
        public Hit(
            Timestamp timestamp, int mvIndex, Func<Timestamp, SecondPassStatsPage> stats,
             Unit unit, Types type, HitType hitType, 
            string description = "", Enemy target = null): 
            base(
            timestamp,

            // TODO We'll need to hook in reactions, enemies, multi-target and infusion here
            // TODO reaction is scuffed and only used for transformative reactions so i want to remove it
            (world) => world.CalculateDamage(
                timestamp, mvIndex, stats(timestamp), 
                unit, type, hitType, description), description
        ) {
        }
        

        /*public Hit(
            Timestamp timestamp, Func<Element> element, int mvIndex, Func<Timestamp, SecondPassStatsPage> stats,
            Unit unit, Types type, HitType hitType, 
            string description = "", Enemy target = null): 
            base(
                timestamp,

                // TODO We'll need to hook in reactions, enemies, multi-target and infusion here
                // TODO reaction is scuffed and only used for transformative reactions so i want to remove it
                (world) => world.CalculateDamage(
                    timestamp, element(), mvIndex, stats(timestamp), 
                    unit, type, hitType, description), description
            ) {
        }*/
    }
}