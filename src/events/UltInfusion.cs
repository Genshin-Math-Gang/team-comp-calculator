using System;
using Tcc.Elements;
using Tcc.Units;

namespace Tcc.Events
{
    public class AbilityInfusion: WorldEvent
    {

        
        public AbilityInfusion(Timestamp start, Timestamp end, Anemo unit) : 
            base(start, world => world.InfuseAbility(start, end, unit))
        { }
    }
}