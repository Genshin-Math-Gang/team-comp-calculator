using System;
using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Overload: WorldEvent
    {
        public Overload(
            Timestamp timestamp, Stats.Stats stats, Units.Unit unit): 
            base(
            timestamp,

            (world) => world.Hit(
                timestamp, Element.PYRO, 0, stats, 
                unit, Types.TRANSFORMATIVE, Reaction.OVERLOADED, true, false, true, 1, "overload")
        ) {
        }
    }
}