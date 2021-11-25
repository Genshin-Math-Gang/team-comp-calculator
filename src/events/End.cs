namespace Tcc.events
{
    public class End: WorldEvent
    {
        public End(Timestamp timestamp): 
            base(timestamp, world => world.ClearQueue()) {}
    }
}