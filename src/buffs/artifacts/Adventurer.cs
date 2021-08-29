using System;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class Adventurer: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("178e67ea-44c9-41fd-9e2b-73e3d879d526");

        static readonly CapacityModifier MODIFIER_2PC = () => new CapacityStats(flatHp: 1000);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<CapacityModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => throw new NotImplementedException();
    }
}
