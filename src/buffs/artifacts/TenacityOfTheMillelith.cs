using System;
using Tcc.Stats;
using Tcc.Units;
using Tcc.Weapons;

namespace Tcc.Buffs.Artifacts
{
    public class TenacityOfTheMillelith: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("12f19925-c85e-4f15-857f-7d305dfb179f");
        static readonly Guid ID_4PC = new Guid("1c5c261f-ab76-4b38-8f59-fcbf829dc58f");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(hpPercent: 0.2);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(attackPercent: 0.2, shieldStrength: 0.3);

        Timestamp cooldown4pcUntil;

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicUnconditionalBuff(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            world.enemyHitHook += (_, data) =>
            {
                if(!data.attackType.IsType(Stats.Types.SKILL)) return;

                if(cooldown4pcUntil != null && data.timestamp < cooldown4pcUntil) return;
                else cooldown4pcUntil = data.timestamp + 0.5;

                foreach(var unitInParty in world.GetUnits()) unitInParty.AddBuff(new RefreshableBuff(ID_4PC, data.timestamp + 3, MODIFIER_4PC));
            };
        }
    }
}
