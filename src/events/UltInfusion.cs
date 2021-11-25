using Tcc.units;

namespace Tcc.events
{
    public class AbilityInfusion: WorldEvent
    {
        
        public AbilityInfusion(Timestamp start, Timestamp end, Anemo unit) : 
            base(start, world => world.InfuseAbility(start, end, unit))
        { }
    }
}