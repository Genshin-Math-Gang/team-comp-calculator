using System;
using Tcc.buffs;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class Instructor: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("3c24bd1c-b4d4-4155-a61a-033cf354af41");
        static readonly Guid ID_4PC = new Guid("7d841450-d57e-41c5-974f-d3d3366bf8d1");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Stats.ElementalMastery, 80);
        static readonly FirstPassModifier MODIFIER_4PC = (_) => (Stats.ElementalMastery, 120);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.TriggeredReactionHook += (_, data) =>
            {
                foreach(var unitInParty in world.GetUnits()) unitInParty.AddBuff(new RefreshableBuff<FirstPassModifier>(ID_4PC, data.timestamp + 8, MODIFIER_4PC));
            };
        }
    }
}
