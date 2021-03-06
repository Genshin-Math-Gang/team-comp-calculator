using System;
using Tcc.buffs;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class BloodstainedChivalry: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("2b17e4b6-d3fa-485b-871f-eb44e6d008a6");

        static readonly FirstPassModifier MODIFIER_2PC = _ => (Stats.PhysicalDamageBonus, 0.25);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        // TODO implement this
        public override void Add4pc(World world, Unit unit) => throw new NotImplementedException();
    }
}
