using System;

namespace Tcc.Buffs.Artifacts
{
    public class TravelingDoctor2pc: BasicUnconditionalBuff
    {
        static readonly Guid ID = new Guid("32a7001f-ff6d-4b38-9b67-be23a1f0db42");
        static readonly Stats.Stats MODIFIER = new Stats.Stats(incomingHealingBonus: 0.2);

        public TravelingDoctor2pc(): base(ID, MODIFIER)
        {
        }
    }
}
