using System;

namespace Tcc.Buffs.Artifacts
{
    public class MartialArtist2pc: BasicUnconditionalBuff
    {
        static readonly Guid ID = new Guid("3a34b09e-0ac2-44e5-a375-1cd19c903f0d");
        static readonly Stats.Stats MODIFIER = new Stats.Stats(damagePercent: 0.15);

        public MartialArtist2pc(): base(ID, MODIFIER, Stats.Types.NORMAL | Stats.Types.CHARGED)
        {
        }
    }

    public class MartialArtist4pc: RefreshableBuff
    {
        static readonly Guid ID = new Guid("a971cb7a-5c2a-4944-a087-6024f40dcc4a");
        static readonly Stats.Stats MODIFIER = new Stats.Stats(damagePercent: 0.25);

        public MartialArtist4pc(): base(ID, new Timestamp(8), MODIFIER, Stats.Types.NORMAL | Stats.Types.CHARGED)
        {
        }
    }
}
