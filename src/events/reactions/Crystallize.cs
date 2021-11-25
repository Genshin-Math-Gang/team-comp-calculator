using Tcc.elements;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events.reactions
{
    public class Crystallize: WorldEvent
    {
        public Crystallize(
            Timestamp timestamp, SecondPassStatsPage stats, Unit unit, Element element): 
            // TODO: dumb double swirl and chain swirl 
            base(
                timestamp,
                // TODO: this is probably wrong but i just want the code to compile rn
                // does crystallize do damage
                (world) => world.CalculateDamage(timestamp, element, 0, stats, unit, Types.TRANSFORMATIVE, 
                    new HitType(false, 1, false, true,CrystalType(element)), "Crystallize")
            ) {
        }

        public static Reaction CrystalType(Element element)
        {
            return element switch
            {
                Element.PYRO => Reaction.CRYSTALLIZE_PYRO,
                Element.CRYO => Reaction.CRYSTALLIZE_CRYO,
                Element.HYDRO => Reaction.CRYSTALLIZE_HYDRO,
                Element.ELECTRO => Reaction.CRYSTALLIZE_ELECTRO
            };
        }
    }
}