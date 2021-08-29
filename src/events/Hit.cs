using System;
using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Hit: WorldEvent
    {
        public Hit(
            Timestamp timestamp, Element element, int mvIndex, Func<Timestamp, SecondPassStatsPage> stats,
             Units.Unit unit, Types type, bool isHeavy = false, bool applyElement = false,
              bool isAoe = true, int bounces = 1, string description = ""): 
            base(
            timestamp,

            // TODO We'll need to hook in reactions, enemies, multi-target and infusion here
            (world) => world.Hit(
                timestamp, element, mvIndex, stats(timestamp), 
                unit, type, Reaction.NONE, isHeavy, isAoe, applyElement, bounces, description)
        ) {
        }
    }
}