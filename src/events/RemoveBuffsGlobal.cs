using System;
using Tcc.Stats;

namespace Tcc.Events
{
    public class RemoveBuffGlobal: WorldEvent
    {
        public RemoveBuffGlobal(double timestamp, string name, Types type, string description = ""): base(
            timestamp,
            (world) => world.RemoveBuffGlobal(timestamp, name, type, description)
        ) {
        }
    }
}