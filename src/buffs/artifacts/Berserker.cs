using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class Berserker: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("70f619bf-a99d-4797-9bee-555e503102d7");
        static readonly Guid ID_4PC = new Guid("b62bfb7b-a12e-42f2-9771-df4d54da8752");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(critRate: 0.12);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(critRate: 0.24);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.AddBuff(new BasicBuffFromStats(ID_4PC, MODIFIER_4PC, condition: (_, stats, _) => unit.CurrentHp / stats.MaxHp < 0.7));
        }
    }
}
