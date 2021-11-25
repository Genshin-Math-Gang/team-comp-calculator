using System;
using Tcc.buffs;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class RetracingBolide: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("cccbadd8-9df1-4b4a-b5a5-2e5e7c2af191");
        static readonly Guid ID_4PC = new Guid("9574a114-a0b4-442c-8c4b-828eff8a34ad");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Stats.ShieldStrength, 0.3);

        private static readonly AbilityModifier MODIFIER_4PC = (data) =>
            data.st.IsShielded ? (Stats.DamagePercent, 0.4) : new StatsPage();

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<AbilityModifier>(ID_4PC, MODIFIER_4PC), Types.NORMAL, Types.CHARGED);
    }
}
