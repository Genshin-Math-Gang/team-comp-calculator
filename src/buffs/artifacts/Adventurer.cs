using System;

namespace Tcc.Buffs.Artifacts
{
    public class Adventurer2pc: BasicUnconditionalBuff
    {
        static readonly Guid ID = new Guid("178e67ea-44c9-41fd-9e2b-73e3d879d526");
        static readonly Stats.Stats MODIFIER = new Stats.Stats(flatHp: 1000);

        public Adventurer2pc(): base(ID, MODIFIER)
        {
        }
    }
}
