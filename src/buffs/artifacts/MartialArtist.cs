using System;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class MartialArtist: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("3a34b09e-0ac2-44e5-a375-1cd19c903f0d");
        static readonly Guid ID_4PC = new Guid("a971cb7a-5c2a-4944-a087-6024f40dcc4a");

        static readonly AbilityModifier MODIFIER_2PC = (_) => new GeneralStats(damagePercent: 0.15);
        static readonly AbilityModifier MODIFIER_4PC = (_) => new GeneralStats(damagePercent: 0.25);

        public override void Add2pc(World world, Unit unit)
            => unit.AddBuff(new PermanentBuff<AbilityModifier>(ID_2PC, MODIFIER_2PC), Stats.Types.NORMAL, Stats.Types.CHARGED);
        
        public override void Add4pc(World world, Unit unit)
        {
            unit.skillActivatedHook += (_, timestamp)
                => unit.AddBuff(new RefreshableBuff<AbilityModifier>(ID_4PC, timestamp + 8, MODIFIER_4PC), Stats.Types.NORMAL, Stats.Types.CHARGED);
        }
    }
}
