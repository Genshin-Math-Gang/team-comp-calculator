namespace Tcc.Events
{
    public class End: WorldEvent
    {
        public End(Timestamp timestamp): 
            base(timestamp, world => world.ClearQueue()) {}
    }
}