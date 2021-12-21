using Tcc.elements;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events.reactions
{
    public class Shatter: WorldEvent
    {

        private static readonly HitType shatterHitType = new HitType(Element.PHYSICAL, reaction: Reaction.SHATTERED);
        // TODO: is shatter heavy
        public Shatter(
            Timestamp timestamp, SecondPassStatsPage stats, Unit unit):
            base(
                timestamp, (world) => world.CalculateDamage(timestamp, 0, stats, unit, 
                    Types.TRANSFORMATIVE, shatterHitType, "Shatter")
            ) {
            
        }
    }
}