using Tcc.units;

namespace Tcc.events
{
    public class AbilityInfusion: WorldEvent
    {
        
        public AbilityInfusion(double start, double end, Anemo unit) : 
            base(start, world => world.InfuseAbility(start, end, unit))
        { }
    }
}