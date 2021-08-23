using System;
using Tcc.Elements;

namespace Tcc.Events
{
    public class Hit: WorldEvent
    {
        public Hit(Timestamp timestamp, Element element, Func<Enemy.Enemy, Timestamp, Stats.Stats> stats, int mvIndex, Units.Unit unit, string description = ""): base(
            timestamp,
            // TODO We'll need to hook in reactions, enemies, multi-target and infusion here
            (world) => world.Hit(timestamp, stats(null, timestamp).CalculateHitDamage(mvIndex, element, null), unit, description)
        ) {
        }
    }
}