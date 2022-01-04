using Tcc.elements;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events.reactions
{
    public class ElectroCharged: WorldEvent
    {
        private static readonly HitType ecType = new HitType(Element.ELECTRO, false, 3, reaction: Reaction.ELECTROCHARGED);
        public ElectroCharged(
            double timestamp, SecondPassStatsPage stats, Unit unit): 
            // TODO: check how many times it bounces
            // Does EC apply electro to nearby enemies
            base(
                timestamp, (world) =>
                {
                    world.CalculateDamage(timestamp, 0, stats, unit,
                        Types.TRANSFORMATIVE, ecType, "Electrocharged");
                    unit.ReactionTriggered(timestamp, Reaction.ELECTROCHARGED);
                }
            ) {
        }
    }
}