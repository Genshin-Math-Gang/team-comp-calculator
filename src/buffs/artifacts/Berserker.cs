using System;

namespace Tcc.Buffs.Artifacts
{
    public class Berserker2pc: BasicUnconditionalBuff
    {
        static readonly Guid ID = new Guid("70f619bf-a99d-4797-9bee-555e503102d7");
        static readonly Stats.Stats MODIFIER = new Stats.Stats(critRate: 0.12);

        public Berserker2pc(): base(ID, MODIFIER)
        {
        }
    }

    public class Berserker4pc: BasicBuffFromUnit
    {
        static readonly Guid ID = new Guid("b62bfb7b-a12e-42f2-9771-df4d54da8752");
        static readonly Stats.Stats MODIFIER = new Stats.Stats(critRate: 0.24);

        public Berserker4pc(): base(ID, MODIFIER, condition: (unit, stats) => unit.CurrentHp < stats.MaxHp * 0.7)
        {
        }
    }
}
