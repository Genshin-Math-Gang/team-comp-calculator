using System;
using Tcc.Buffs;

namespace Tcc.Events
{
    public class AddBuffOnField: WorldEvent
    {
        public AddBuffOnField(Timestamp timestamp, Func<BuffFromUnit> buff, string description = ""): base(
            timestamp,
            (world) => world.AddBuffOnField(timestamp, buff(), description)
        ) {
        }

        public AddBuffOnField(Timestamp timestamp, BuffFromUnit buff, string description = ""): base(
            timestamp,
            (world) => world.AddBuffOnField(timestamp, buff, description)
        ) {
        }
    }
}