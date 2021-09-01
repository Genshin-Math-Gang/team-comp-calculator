using System;
using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Swirl: WorldEvent
    {
        public Swirl(
            Timestamp timestamp, SecondPassStatsPage stats, Units.Unit unit, Element element): 
            // TODO: dumb double swirl and chain swirl 
            base(
                timestamp,
                
                (world) => world.CalculateDamage(timestamp, element, 0, stats, unit, 
                    Types.TRANSFORMATIVE, SwirlType(element), false,true, 2, -1, "Swirl")
            ) {
            
        }

        public static Reaction SwirlType(Element element)
        {
            return element switch
            {
                Element.PYRO => Reaction.SWIRL_PYRO,
                Element.CRYO => Reaction.SWIRL_CRYO,
                Element.HYDRO => Reaction.SWIRL_HYDRO,
                Element.ELECTRO => Reaction.SWIRL_ELECTRO
            };
        }
    }
}