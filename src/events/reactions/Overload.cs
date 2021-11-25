using Tcc.elements;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events.reactions
{
    public class Overload: WorldEvent
    {
        public Overload(
            Timestamp timestamp, SecondPassStatsPage stats, Unit unit): 
            base(
            timestamp, (world) => world.CalculateDamage(timestamp, Element.PYRO, 0, stats, unit, Types.TRANSFORMATIVE,
                new HitType(true, 1, false, true, Reaction.OVERLOADED), "Overload")
        ) {
        }
    }
}