using System;
using Tcc.Elements;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class BloodstainedChivalry: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("2b17e4b6-d3fa-485b-871f-eb44e6d008a6");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Element.PHYSICAL, 0.25);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => throw new NotImplementedException();
    }
}
