using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Superconduct: WorldEvent
    {
        public Superconduct(
            Timestamp timestamp, SecondPassStatsPage stats, Units.Unit unit):
            base(
                timestamp,
                (world) => world.CalculateDamage(timestamp, Element.CRYO, 0, stats, unit, 
                    Types.TRANSFORMATIVE, Reaction.SUPERCONDUCT, false,true, 1, -1, "Superconduct")
            ) {
            
            // debuff enemy res
        }
    }
}