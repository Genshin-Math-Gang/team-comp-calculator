using System;
using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Crystallize: WorldEvent
    {
        public Crystallize(
            Timestamp timestamp, SecondPassStatsPage stats, Units.Unit unit, Element element): 
            // TODO: dumb double swirl and chain swirl 
            base(
                timestamp,
                
                (world) => world.CalculateDamage(timestamp, element, 0, stats, unit, 
                    Types.TRANSFORMATIVE, CrystalType(element), false,true, 1, -1)
            ) {
        }

        public static Reaction CrystalType(Element element)
        {
            return element switch
            {
                Element.PYRO => Reaction.CRYSTALLIZE_CRYO,
                Element.CRYO => Reaction.CRYSTALLIZE_CRYO,
                Element.HYDRO => Reaction.CRYSTALLIZE_HYDRO,
                Element.ELECTRO => Reaction.CRYSTALLIZE_ELECTRO
            };
        }
    }
}