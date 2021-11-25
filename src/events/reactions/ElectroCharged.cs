using Tcc.elements;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events.reactions
{
    public class ElectroCharged: WorldEvent
    {
        public ElectroCharged(
            Timestamp timestamp, SecondPassStatsPage stats, Unit unit): 
            // TODO: check how many times it bounces
            // Does EC apply electro to nearby enemies
            base(
                timestamp, (world) => world.CalculateDamage(timestamp, Element.ELECTRO, 0, stats, unit, Types.TRANSFORMATIVE, 
                    new HitType(false, 3, false, false, Reaction.ELECTROCHARGED), "Electrocharged")
            ) {
        }
    }
}