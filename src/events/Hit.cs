using System;
using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Hit: WorldEvent
    {
        public Hit(
            Timestamp timestamp, Element element, int mvIndex, Func<Timestamp, SecondPassStatsPage> stats,
             Units.Unit unit, Types type, HitType hitType, 
            string description = "", Enemy.Enemy target = null): 
            base(
            timestamp,

            // TODO We'll need to hook in reactions, enemies, multi-target and infusion here
            // TODO reaction is scuffed and only used for transformative reactions so i want to remove it
            (world) => world.CalculateDamage(
                timestamp, element, mvIndex, stats(timestamp), 
                unit, type, hitType, description), description
        ) {
        }
    }
}