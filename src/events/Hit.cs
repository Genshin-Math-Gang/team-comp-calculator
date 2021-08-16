using System;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Hit: WorldEvent
    {
        public Hit(Timestamp timestamp, Func<Timestamp, Stats.Stats> stats, int index, Units.Unit unit, string description = ""): base(
            timestamp,
            (world) => world.Hit(timestamp, stats(timestamp).CalculateHitDamage(index), unit, description)
        ) {
        }
    }
}