using System;

namespace Tcc.Buffs.Artifacts
{
    public class Gambler2pc: BasicUnconditionalBuff
    {
        static readonly Guid ID = new Guid("46c94496-8ccb-4565-9ac3-72638b07bb4a");
        static readonly Stats.Stats MODIFIER = new Stats.Stats(damagePercent: 0.2);

        public Gambler2pc(): base(ID, MODIFIER, Stats.Types.SKILL)
        {
        }
    }
}
