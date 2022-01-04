namespace Tcc.events
{
    public class End: WorldEvent
    {
        public End(double timestamp): 
            base(timestamp, world => world.ClearQueue()) {}
    }
}