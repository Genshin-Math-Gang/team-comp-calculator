using System;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Hit: WorldEvent
    {
        public Hit(double timestamp, Func<UnitStats> stats): base(
            timestamp,
            (world) => world.DealDamage(stats().CalculateHitDamage())
        ) {
        }
    }
}