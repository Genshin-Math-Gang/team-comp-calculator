using Tcc.Buffs;

namespace Tcc.Events
{
    public class AddBuffGlobal: WorldEvent
    {
        public AddBuffGlobal(Timestamp timestamp, UnconditionalBuff buff, string description = ""): base(
            timestamp,
            (world) => world.AddBuffGlobal(timestamp, buff, description)
        ) {
        }
    }
}