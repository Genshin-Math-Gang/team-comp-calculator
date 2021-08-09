using System;

namespace Tcc.Events
{
    public class WorldEvent
    {
        readonly Action<World> effect;

        public WorldEvent(double timestamp, Action<World> effect)
        {
            this.Timestamp = timestamp;
            this.effect = effect;
        }

        public double Timestamp { get; }

        public void Apply(World world) => effect(world);
    }
}
