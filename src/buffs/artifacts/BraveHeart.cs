using System;

namespace Tcc.Buffs.Artifacts
{
    public class BraveHeart2pc: BasicUnconditionalBuff
    {
        static readonly Guid ID = new Guid("d346bb43-a398-417d-813e-9c2b4cf42704");
        static readonly Stats.Stats MODIFIER = new Stats.Stats(attackPercent: 0.18);

        public BraveHeart2pc(): base(ID, MODIFIER)
        {
        }
    }

    // public class BraveHeart4pc: BasicBuffFromEnemy
    // {
    //     static readonly Guid ID = new Guid("c90588f9-98a0-4387-b0ac-0e61f46ed519");
    //     static readonly Stats.Stats MODIFIER = new Stats.Stats(damagePercent: 0.3);

    //     public BraveHeart4pc(): base(ID, (enemy) => enemy.CurrentHp > enemy.MaxHp, MODIFIER)
    //     {
    //     }
    // }
}
