using System;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Hit: WorldEvent
    {
        public Hit(double timestamp, Func<Stats.Stats> stats, int index, Units.Unit unit, string description = ""): base(
            timestamp,
            (world) => world.Hit(timestamp, stats().CalculateHitDamage(index), unit, description)
        ) {
        }
    }
}