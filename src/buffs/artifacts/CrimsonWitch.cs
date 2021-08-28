using System;
using Tcc.Elements;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class CrimsonWitch: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("eb489029-e2e5-4028-b119-da6436ef24a0");
        static readonly Guid ID_4PC_PASSIVE = new Guid("608f7b4e-96a3-4731-954a-6ca247ade87e");
        static readonly Guid ID_4PC_STACK = new Guid("73e3f42e-bfe4-4592-ac31-3db20097d8cb");

        static readonly KeyedPercentBonus<Element> MODIFIER_2PC = new KeyedPercentBonus<Element>(Element.PYRO, 0.15);

        static readonly Stats.Stats MODIFIER_4PC_PASSIVE = new KeyedPercentBonus<Reaction>(
            (Reaction.OVERLOADED, 40),
            (Reaction.BURNING, 40),
            (Reaction.MELT, 15),
            (Reaction.VAPORIZE, 15)
        );

        static readonly Stats.Stats MODIFIER_4PC_STACK = 0.5 * MODIFIER_2PC;

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.AddBuff(new BasicBuffFromUnit(ID_4PC_PASSIVE, MODIFIER_4PC_PASSIVE));

            unit.skillActivatedHook += (_, timestamp) => unit.AddBuff(
                new RefreshableBuff(ID_4PC_STACK, timestamp + 10, MODIFIER_4PC_STACK, maxStacks: 3)
            );
        }
    }
}
