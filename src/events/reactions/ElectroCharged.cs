using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class ElectroCharged: WorldEvent
    {
        public ElectroCharged(
            Timestamp timestamp, SecondPassStatsPage stats, Units.Unit unit): 
            // TODO: change damage type since idk what it is
            base(
                timestamp,
                
                (world) => world.CalculateDamage(timestamp, Element.PYRO, 0, stats, unit, 
                    Types.TRANSFORMATIVE, Reaction.ELECTROCHARGED, false,true, 1, -1)
            ) {
        }
    }
}