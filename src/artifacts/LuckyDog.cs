using System;
using Tcc.buffs;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    // imagine using this fuckjing set
    public class LuckyDog: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("063493f0-f5ca-472a-8b9f-20c3fc4072ad");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Stats.DefFlat, 100);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => throw new NotImplementedException("get fucked im not implementing this");
    }
}
