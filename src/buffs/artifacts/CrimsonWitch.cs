using System;
using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Buffs.Artifacts
{
    public class CrimsonWitch2pc: BasicBuffFromUnit
    {
        static readonly Guid ID = new Guid("eb489029-e2e5-4028-b119-da6436ef24a0");
        static readonly Stats.Stats MODIFIER = new KeyedPercentBonus<Element>(Element.PYRO, 0.15);

        public CrimsonWitch2pc(): base(ID, MODIFIER)
        {
        }
    }
}