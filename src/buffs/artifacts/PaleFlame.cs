using System;
using Tcc.Elements;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class PaleFlame: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("393837b5-15e5-4e3a-800f-339d87506ff5");
        static readonly Guid ID_4PC_STACK = new Guid("bdf450e0-8152-49c3-ab80-3f86225ce741");
        static readonly Guid ID_4PC_CONDITIONAL = new Guid("b5c41036-6c67-465f-8337-36fdc152abab");

        static readonly Stats.Stats MODIFIER_2PC = new KeyedPercentBonus<Element>(Element.PHYSICAL, 0.25);
        static readonly Stats.Stats MODIFIER_4PC_STACK = new Stats.Stats(attackPercent: 0.09);
        static readonly Stats.Stats MODIFIER_4PC_CONDITIONAL = MODIFIER_2PC;

        Timestamp cooldown4pcUntil;

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicUnconditionalBuff(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.AddBuff(new BasicBuffFromUnit(ID_4PC_CONDITIONAL, MODIFIER_4PC_CONDITIONAL, condition: (unit, _) => unit.GetBuffCount(ID_4PC_STACK) == 2));

            world.enemyHitHook += (_, data) =>
            {
                if(!data.attackType.IsType(Stats.Types.SKILL)) return;

                if(cooldown4pcUntil != null && data.timestamp < cooldown4pcUntil) return;
                else cooldown4pcUntil = data.timestamp + 0.3;

                unit.AddBuff(new RefreshableBuff(ID_4PC_STACK, data.timestamp + 7, MODIFIER_4PC_STACK, maxStacks: 2));
            };
        }
    }
}
