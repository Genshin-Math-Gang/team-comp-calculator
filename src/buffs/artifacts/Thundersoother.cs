using System;
using Tcc.Elements;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class Thundersoother: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("ca4ba1c2-fb68-4dc3-b113-aad2d40b8af9");
        static readonly Guid ID_4PC = new Guid("febada3a-1976-4d57-99e9-1a88a05201de");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(elementalResistance: new KeyedPercentBonus<Element>(Element.ELECTRO, 0.4));
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(damagePercent: 0.35);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.AddBuff(new BasicBuffFromEnemy(ID_4PC, (enemy) => enemy.HasAura(Aura.ELECTRO), MODIFIER_4PC));
        }
    }
}
