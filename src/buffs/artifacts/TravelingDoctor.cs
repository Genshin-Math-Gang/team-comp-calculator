using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class TravelingDoctor: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("32a7001f-ff6d-4b38-9b67-be23a1f0db42");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(incomingHealingBonus: 0.2);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicUnconditionalBuff(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => throw new NotImplementedException();
    }
}
