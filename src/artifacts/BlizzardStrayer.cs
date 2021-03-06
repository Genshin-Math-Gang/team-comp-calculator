using System;
using Tcc.buffs;
using Tcc.elements;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class BlizzardStrayer: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("9da2cd66-4823-49ba-911e-49cb3e84a2f3");
        static readonly Guid ID_4PC = new Guid("b995f2bd-99be-4abe-8ea9-3d2c8eedea4a");

        static readonly FirstPassModifier MODIFIER_2PC = _ => new StatsPage(Stats.CryoDamageBonus, 0.15);

        static readonly EnemyBasedModifier MODIFIER_4PC = (data) =>
        {
            if (data.enemy.HasAura(Aura.FROZEN)) return new StatsPage(Stats.CritRate, 0.4);
            if (data.enemy.HasAura(Aura.CRYO)) return new StatsPage(Stats.CritRate, 0.4);
            return new StatsPage();
        };

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<EnemyBasedModifier>(ID_4PC, MODIFIER_4PC));
    }
}
