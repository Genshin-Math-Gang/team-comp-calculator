using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class Gambler: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("46c94496-8ccb-4565-9ac3-72638b07bb4a");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(damagePercent: 0.2);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_2PC, MODIFIER_2PC, Stats.Types.SKILL));
        public override void Add4pc(World world, Unit unit) => throw new NotImplementedException();
    }
}
