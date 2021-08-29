using System;
using Tcc.Elements;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class BlizzardStrayer: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("9da2cd66-4823-49ba-911e-49cb3e84a2f3");
        static readonly Guid ID_4PC = new Guid("b995f2bd-99be-4abe-8ea9-3d2c8eedea4a");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Element.CRYO, 0.15);

        static readonly EnemyBasedModifier MODIFIER_4PC = (data) =>
        {
            if (data.enemy.HasAura(Aura.FROZEN)) return new GeneralStats(critRate: 0.4);
            if (data.enemy.HasAura(Aura.CRYO)) return new GeneralStats(critRate: 0.2);
            return new GeneralStats();
        };

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<EnemyBasedModifier>(ID_4PC, MODIFIER_4PC));
    }
}
