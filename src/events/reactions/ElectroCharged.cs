using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class ElectroCharged: WorldEvent
    {
        public ElectroCharged(
            Timestamp timestamp, SecondPassStatsPage stats, Units.Unit unit): 
            // TODO: check how many times it bounces
            base(
                timestamp,
                
                (world) => world.CalculateDamage(timestamp, Element.ELECTRO, 0, stats, unit, 
                    Types.TRANSFORMATIVE, Reaction.ELECTROCHARGED, false,true, 3, "Electrocharged")
            ) {
        }
    }
}