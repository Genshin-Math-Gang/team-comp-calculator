using System;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class BraveHeart: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("d346bb43-a398-417d-813e-9c2b4cf42704");
        static readonly Guid ID_4PC = new Guid("c90588f9-98a0-4387-b0ac-0e61f46ed519");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => new GeneralStats(attackPercent: 0.18);
        static readonly EnemyBasedModifier MODIFIER_4PC = (data) => throw new NotImplementedException();
        //static readonly EnemyBasedModifier MODIFIER_4PC = (data) => data.enemy.CurrentHp / data.enemy.MaxHp > 0.5 ? new DamagePercentAndStats(damagePercentBonus: 0.3) : null;

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<EnemyBasedModifier>(ID_4PC, MODIFIER_4PC));
    }
}
