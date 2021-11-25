using Tcc.elements;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events.reactions
{
    public class Shatter: WorldEvent
    {
        public Shatter(
            Timestamp timestamp, SecondPassStatsPage stats, Unit unit):
            base(
                timestamp, (world) => world.CalculateDamage(timestamp, Element.PHYSICAL, 0, stats, unit, Types.TRANSFORMATIVE, 
                    new HitType(false, 1, false, false, Reaction.SHATTERED), "Shatter")
            ) {
            
        }
    }
}