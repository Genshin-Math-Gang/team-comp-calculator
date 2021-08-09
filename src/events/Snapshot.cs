using System;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Snapshot: WorldEvent
    {
        public Snapshot(double timestamp, Units.Unit unit, Types type, string description = ""): base(
            timestamp,
            (world) => world.Snapshot(timestamp, unit, type, description)
        ) {
        }
    }
}