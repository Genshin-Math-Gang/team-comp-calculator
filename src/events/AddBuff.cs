using Tcc.Buffs;

namespace Tcc.Events
{
    public class AddBuff: WorldEvent
    {
        public AddBuff(Timestamp timestamp, Units.Unit unit, BuffFromUnit buff, string description = ""): base(
            timestamp,
            (world) => world.AddBuff(timestamp, unit, buff, description)
        ) {
        }
    }
}