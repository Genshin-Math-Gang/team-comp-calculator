using System;

namespace Tcc.Buffs.Artifacts
{
    public class LuckyDog2pc: BasicUnconditionalBuff
    {
        static readonly Guid ID = new Guid("063493f0-f5ca-472a-8b9f-20c3fc4072ad");
        static readonly Stats.Stats MODIFIER = new Stats.Stats(flatDefence: 100);

        public LuckyDog2pc(): base(ID, MODIFIER)
        {
        }
    }
}
