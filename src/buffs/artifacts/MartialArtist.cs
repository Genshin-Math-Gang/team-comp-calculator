using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class MartialArtist: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("3a34b09e-0ac2-44e5-a375-1cd19c903f0d");
        static readonly Guid ID_4PC = new Guid("a971cb7a-5c2a-4944-a087-6024f40dcc4a");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(damagePercent: 0.15);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(damagePercent: 0.25);

        public override void Add2pc(World world, Unit unit)
        {
            unit.AddBuff(new BasicUnconditionalBuff(ID_2PC, MODIFIER_2PC, Stats.Types.NORMAL | Stats.Types.CHARGED));
        }

        public override void Add4pc(World world, Unit unit)
        {
            unit.skillActivatedHook += (_, timestamp) => unit.AddBuff(
                new RefreshableBuff(ID_4PC, timestamp + 8, MODIFIER_4PC, Stats.Types.NORMAL | Stats.Types.CHARGED)
            );
        }
    }
}
