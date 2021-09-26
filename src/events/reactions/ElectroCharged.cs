using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class ElectroCharged: WorldEvent
    {
        public ElectroCharged(
            Timestamp timestamp, SecondPassStatsPage stats, Units.Unit unit): 
            // TODO: check how many times it bounces
            // Does EC apply electro to nearby enemies
            base(
                timestamp, (world) => world.CalculateDamage(timestamp, Element.ELECTRO, 0, stats, unit, Types.TRANSFORMATIVE, 
                    new HitType(false, 3, false, false, Reaction.ELECTROCHARGED), "Electrocharged")
            ) {
        }
    }
}