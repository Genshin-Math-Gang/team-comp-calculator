using System;
using Tcc.buffs;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class Gambler: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("46c94496-8ccb-4565-9ac3-72638b07bb4a");

        static readonly AbilityModifier MODIFIER_2PC = (_) => (Stats.DamagePercent, 0.2);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<AbilityModifier>(ID_2PC, MODIFIER_2PC), Types.SKILL);
        public override void Add4pc(World world, Unit unit) => throw new NotImplementedException();
    }
}
