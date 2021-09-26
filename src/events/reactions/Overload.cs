using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Overload: WorldEvent
    {
        public Overload(
            Timestamp timestamp, SecondPassStatsPage stats, Units.Unit unit): 
            base(
            timestamp, (world) => world.CalculateDamage(timestamp, Element.PYRO, 0, stats, unit, Types.TRANSFORMATIVE,
                new HitType(true, 1, false, true, Reaction.OVERLOADED), "Overload")
        ) {
        }
    }
}