using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class HeartOfDepth: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("3eac5874-0066-479d-aae7-3e1f8112bec6");
        static readonly Guid ID_4PC = new Guid("84b664c7-5851-4790-98ee-ad2af4817786");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(damagePercent: 0.15);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(damagePercent: 0.3);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicUnconditionalBuff(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.skillActivatedHook += (_, timestamp) => unit.AddBuff(
                new RefreshableBuff(ID_4PC, timestamp + 15, MODIFIER_4PC, Stats.Types.NORMAL | Stats.Types.CHARGED)
            );
        }
    }
}
