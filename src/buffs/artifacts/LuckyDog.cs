using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class LuckyDog: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("063493f0-f5ca-472a-8b9f-20c3fc4072ad");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(flatDefence: 100);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => throw new NotImplementedException();
    }
}
