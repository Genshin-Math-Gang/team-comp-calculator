using System;
using Tcc.Elements;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class Lavawalker: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("1eaefb3e-6207-44ba-83c2-bb034fec794a");
        static readonly Guid ID_4PC = new Guid("f9e7a1af-4961-42f1-b9ff-293b25baf611");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Stats.Stats.PyroResistance, 0.4);
        static readonly EnemyBasedModifier MODIFIER_4PC = (data) => data.enemy.HasAura(Aura.PYRO) ? 
            (Stats.Stats.DamagePercent, 0.35) : new StatsPage();

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<EnemyBasedModifier>(ID_4PC, MODIFIER_4PC));
    }
}
