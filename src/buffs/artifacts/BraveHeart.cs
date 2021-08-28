using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class BraveHeart: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("d346bb43-a398-417d-813e-9c2b4cf42704");
        static readonly Guid ID_4PC = new Guid("c90588f9-98a0-4387-b0ac-0e61f46ed519");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(attackPercent: 0.18);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(damagePercent: 0.3);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => throw new NotImplementedException();
        // public override void Add4pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromEnemy(ID_4PC, (enemy) => enemy.CurrentHp > enemy.MaxHp, MODIFIER_4PC))
    }
}
