using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class ResolutionOfSojourner: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("b8b67d3b-4089-4019-94ca-d089fcd10665");
        static readonly Guid ID_4PC = new Guid("6b43cda3-9b8f-4403-8c09-c2ea722f53a3");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(attackPercent: 0.18);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(critRate: 0.3);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_4PC, MODIFIER_4PC, Stats.Types.CHARGED));
    }
}
