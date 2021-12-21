using Tcc.elements;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events.reactions
{
    public class Overload: WorldEvent
    {

        private static readonly HitType overloadHitType = new HitType(Element.PYRO, heavy:true, reaction: Reaction.OVERLOADED);
        public Overload(
            Timestamp timestamp, SecondPassStatsPage stats, Unit unit): 
            base(
            timestamp, (world) => world.CalculateDamage(timestamp, 0, stats, unit, 
                Types.TRANSFORMATIVE, overloadHitType), "Overload") 
        {
        }
    }
}