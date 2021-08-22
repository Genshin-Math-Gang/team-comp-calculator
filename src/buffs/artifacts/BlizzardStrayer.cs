using System;
using Tcc.Elements;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class BlizzardStrayer: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("9da2cd66-4823-49ba-911e-49cb3e84a2f3");
        static readonly Guid ID_4PC_IF_CRYO = new Guid("b995f2bd-99be-4abe-8ea9-3d2c8eedea4a");
        static readonly Guid ID_4PC_IF_FROZEN = new Guid("a9490516-9222-4071-a2d8-82fd2e14c72a");

        static readonly Stats.Stats MODIFIER_2PC = new KeyedPercentBonus<Element>(Element.CRYO, 0.15);
        static readonly Stats.Stats MODIFIER_4PC_IF_CRYO = new Stats.Stats(critRate: 0.2);
        static readonly Stats.Stats MODIFIER_4PC_IF_FROZEN = new Stats.Stats(critRate: 0.2);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicUnconditionalBuff(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.AddBuff(new BasicBuffFromEnemy(ID_4PC_IF_CRYO, (enemy) => enemy.HasAura(Aura.CRYO), MODIFIER_4PC_IF_CRYO));
            unit.AddBuff(new BasicBuffFromEnemy(ID_4PC_IF_FROZEN, (enemy) => enemy.HasAura(Aura.FROZEN), MODIFIER_4PC_IF_FROZEN));
        }
    }
}
