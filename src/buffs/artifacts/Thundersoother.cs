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

        static readonly FirstPassModifier MODIFIER_2PC = (_) => new GeneralStats(elementalResistance: new KeyedPercentBonus<Element>(Element.ELECTRO, 0.4));
        static readonly EnemyBasedModifier MODIFIER_4PC = (data) => data.enemy.HasAura(Aura.ELECTRO) ? new GeneralStats(damagePercent: 0.35) : new GeneralStats();

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<EnemyBasedModifier>(ID_4PC, MODIFIER_4PC));
    }
}
