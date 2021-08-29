using System;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class RetracingBolide: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("cccbadd8-9df1-4b4a-b5a5-2e5e7c2af191");
        static readonly Guid ID_4PC = new Guid("9574a114-a0b4-442c-8c4b-828eff8a34ad");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => new GeneralStats(shieldStrength: 0.35);
        static readonly AbilityModifier MODIFIER_4PC = (data) => data.unit.IsShielded ? new GeneralStats(damagePercent: 0.4) : new GeneralStats();

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<AbilityModifier>(ID_4PC, MODIFIER_4PC), Stats.Types.NORMAL, Stats.Types.CHARGED);
    }
}
