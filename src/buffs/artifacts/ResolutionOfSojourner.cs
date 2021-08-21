using System;

namespace Tcc.Buffs.Artifacts
{
    public class ResolutionOfSojourner2pc: BasicUnconditionalBuff
    {
        static readonly Guid ID = new Guid("b8b67d3b-4089-4019-94ca-d089fcd10665");
        static readonly Stats.Stats MODIFIER = new Stats.Stats(attackPercent: 0.18);

        public ResolutionOfSojourner2pc(): base(ID, MODIFIER)
        {
        }
    }

    public class ResolutionOfSojourner4pc: BasicUnconditionalBuff
    {
        static readonly Guid ID = new Guid("6b43cda3-9b8f-4403-8c09-c2ea722f53a3");
        static readonly Stats.Stats MODIFIER = new Stats.Stats(critRate: 0.3);

        public ResolutionOfSojourner4pc(): base(ID, MODIFIER, Stats.Types.CHARGED)
        {
        }
    }
}
