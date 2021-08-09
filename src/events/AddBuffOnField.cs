using System;
using Tcc.Stats;

namespace Tcc.Events
{
    public class AddBuffOnField: WorldEvent
    {
        public AddBuffOnField(double timestamp, Stats.Stats stats, string name, Types type, string description = ""): base(
            timestamp,
            (world) => world.AddBuffOnField(timestamp, stats, name, type, description)
        ) {
        }
    }
}