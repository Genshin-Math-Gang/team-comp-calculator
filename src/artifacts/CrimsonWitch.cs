using System;
using System.Collections.Generic;
using Tcc.Elements;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class CrimsonWitch: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("eb489029-e2e5-4028-b119-da6436ef24a0");
        static readonly Guid ID_4PC_PASSIVE = new Guid("608f7b4e-96a3-4731-954a-6ca247ade87e");
        static readonly Guid ID_4PC_STACK = new Guid("73e3f42e-bfe4-4592-ac31-3db20097d8cb");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Stats.Stats.PyroDamageBonus, 0.15);

        private static readonly FirstPassModifier MODIFIER_4PC_PASSIVE = (_) =>
            new StatsPage(new Dictionary<Stats.Stats, double>
            {
                {Stats.Stats.OverloadBonus, 0.4},
                {Stats.Stats.BurningBonus, 0.4},
                {Stats.Stats.MeltBonus, 0.15},
                {Stats.Stats.VaporizeBonus, 0.15}
            });

        static readonly FirstPassModifier MODIFIER_4PC_STACK = (_) => (Stats.Stats.PyroDamageBonus, 0.075);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_4PC_PASSIVE, MODIFIER_4PC_PASSIVE));

            unit.SkillActivatedHook += (_, timestamp)
                => unit.AddBuff(new RefreshableBuff<FirstPassModifier>(ID_4PC_STACK, timestamp + 10, MODIFIER_4PC_STACK, maxStacks: 3));
        }
    }
}
