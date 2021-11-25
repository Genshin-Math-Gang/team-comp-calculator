using Tcc.elements;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events.reactions
{
    public class Superconduct: WorldEvent
    {
        public Superconduct(
            Timestamp timestamp, SecondPassStatsPage stats, Unit unit):
            base(
                timestamp, (world) => world.CalculateDamage(timestamp, Element.CRYO, 0, stats, unit, Types.TRANSFORMATIVE, 
                    new HitType(true, 1, false, false, Reaction.SUPERCONDUCT), "Superconduct")
            ) {
            
        }
    }
}