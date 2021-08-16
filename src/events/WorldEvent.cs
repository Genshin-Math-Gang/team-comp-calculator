using System;

namespace Tcc.Events
{
    public class WorldEvent
    {
        readonly Action<World> effect;

        public WorldEvent(Timestamp timestamp, Action<World> effect)
        {
            this.Timestamp = timestamp;
            this.effect = effect;
        }

        public Timestamp Timestamp { get; }

        public void Apply(World world) => effect(world);
    }
}
