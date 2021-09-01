using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Shatter: WorldEvent
    {
        public Shatter(
            Timestamp timestamp, SecondPassStatsPage stats, Units.Unit unit):
            base(
                timestamp,
                
                (world) => world.CalculateDamage(timestamp, Element.PHYSICAL, 0, stats, unit, 
                    Types.TRANSFORMATIVE, Reaction.SHATTERED, false,false, 1, -1)
            ) {
        }
    }
}