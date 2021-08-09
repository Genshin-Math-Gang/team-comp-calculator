using System;
using Tcc.Stats;

namespace Tcc.Events
{
    public class SwitchUnit: WorldEvent
    {
        public SwitchUnit(double timestamp, Units.Unit unit): base(
            timestamp,
            (world) => world.SwitchUnit(timestamp, unit)
        ) {
        }
    }
}