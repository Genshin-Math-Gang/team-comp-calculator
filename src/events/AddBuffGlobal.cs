using System;
using Tcc.Stats;

namespace Tcc.Events
{
    public class AddBuffGlobal: WorldEvent
    {
        public AddBuffGlobal(double timestamp, Stats.Stats stats, string name, Types type, string description = ""): base(
            timestamp,
            (world) => world.AddBuffGlobal(timestamp, stats, name, type, description)
        ) {
        }
    }
}