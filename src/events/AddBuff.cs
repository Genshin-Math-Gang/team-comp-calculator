using System;
using Tcc.Stats;

namespace Tcc.Events
{
    public class AddBuff: WorldEvent
    {
        public AddBuff(double timestamp, Stats.Stats stats, Units.Unit unit, string name, Types type, string description = ""): base(
            timestamp,
            (world) => world.AddBuff(timestamp, stats, unit, name, type, description)
        ) {
        }
    }
}