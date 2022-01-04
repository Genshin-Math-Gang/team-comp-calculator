using Tcc.elements;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events.reactions
{
    public class Superconduct: WorldEvent
    {
        private static readonly HitType scHitType = new HitType(Element.CRYO, reaction: Reaction.SUPERCONDUCT);
        public Superconduct(
            Timestamp timestamp, SecondPassStatsPage stats, Unit unit):
            base(
                timestamp, (world) =>
                {
                    world.CalculateDamage(timestamp, 0, stats, unit,
                        Types.TRANSFORMATIVE, scHitType, "Superconduct");
                    unit.ReactionTriggered(timestamp, Reaction.SUPERCONDUCT);
                }
            ) {
            
        }
    }
}