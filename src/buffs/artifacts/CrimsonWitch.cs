using System;
using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Buffs.Artifacts
{
    public class CrimsonWitch2pc: BasicBuffFromUnit
    {
        static readonly Guid ID = new Guid("eb489029-e2e5-4028-b119-da6436ef24a0");
        public static readonly KeyedPercentBonus<Element> MODIFIER = new KeyedPercentBonus<Element>(Element.PYRO, 0.15);

        public CrimsonWitch2pc(): base(ID, MODIFIER)
        {
        }
    }

    public class CrimsonWitch4pcPassive: BasicBuffFromUnit
    {
        static readonly Guid ID = new Guid("608f7b4e-96a3-4731-954a-6ca247ade87e");
        static readonly Stats.Stats MODIFIER = new KeyedPercentBonus<Reaction>(
            (Reaction.OVERLOADED, 40),
            (Reaction.BURNING, 40),
            (Reaction.MELT, 15),
            (Reaction.VAPORIZE, 15)
        );

        public CrimsonWitch4pcPassive(): base(ID, MODIFIER)
        {
        }
    }

    public class CrimsonWitch4pcStack: RefreshableBuff
    {
        static readonly Guid ID = new Guid("73e3f42e-bfe4-4592-ac31-3db20097d8cb");
        static readonly Stats.Stats MODIFIER = 0.5 * CrimsonWitch2pc.MODIFIER;

        public CrimsonWitch4pcStack(Timestamp expiryTime): base(ID, expiryTime, MODIFIER, maxStacks: 3)
        {
        }
    }
}
