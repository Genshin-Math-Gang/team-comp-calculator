using System;

namespace Tcc.Buffs.Characters
{
    public class BennettBurstBuff: RefreshableBuff
    {
        static readonly Guid ID = new Guid("c1a23bde-db12-4589-9baf-d25b76ccb989");
        static readonly Timestamp DURATION = new Timestamp(2);

        public BennettBurstBuff(Stats.Stats modifier, Timestamp startTime)
            : base(ID, startTime + DURATION, modifier)
        {
        }
    }
}
