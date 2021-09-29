using System;
using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Swirl: WorldEvent
    {
        // swirl on its own doesn't bounce, i tested with venti aimed shot on 2 grouped enemies and only 1 swirl instance per target
        // however when an anemo attack with aoe hits all enemies double swirl because swirl has aoe
        public Swirl(
            Timestamp timestamp, SecondPassStatsPage stats, Units.Unit unit, Element element): 
            // TODO: dumb double swirl and chain swirl 
            // TODO: figure out ICD bullshit for transformative reactions
            base(
                timestamp, (world) => world.CalculateDamage(timestamp, element, 0, stats, unit, Types.TRANSFORMATIVE,
                    new HitType(true, 1, false, reaction: SwirlType(element)), "Swirl " + element)
            )
        {
            // bruh
            Descrption = "Swirl " + element;
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