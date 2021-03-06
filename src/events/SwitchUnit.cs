using Tcc.units;

namespace Tcc.events
{
    public class SwitchUnit: WorldEvent
    {
        public SwitchUnit(double timestamp, Unit unit): base(
            timestamp,
            (world) => world.SwitchUnit(timestamp, unit), $"Switched to {unit}"
        ) {
        }
    }
}