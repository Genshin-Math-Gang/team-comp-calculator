using System;
using Tcc.buffs;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class ResolutionOfSojourner: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("b8b67d3b-4089-4019-94ca-d089fcd10665");
        static readonly Guid ID_4PC = new Guid("6b43cda3-9b8f-4403-8c09-c2ea722f53a3");

        static readonly FirstPassModifier MODIFIER_2PC = _ => (Stats.AtkPercent, 0.18);
        static readonly AbilityModifier MODIFIER_4PC = _ => (Stats.CritRate, 0.3);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<AbilityModifier>(ID_4PC, MODIFIER_4PC), Types.CHARGED);
    }
}
