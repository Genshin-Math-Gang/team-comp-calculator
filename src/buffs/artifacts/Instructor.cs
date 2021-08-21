using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class Instructor: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("3c24bd1c-b4d4-4155-a61a-033cf354af41");
        static readonly Guid ID_4PC = new Guid("7d841450-d57e-41c5-974f-d3d3366bf8d1");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(elementalMastery: 80);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(elementalMastery: 120);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicUnconditionalBuff(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.triggeredReactionHook += (_, data) =>
            {
                foreach(var unitInParty in world.GetUnits()) unitInParty.AddBuff(new RefreshableBuff(ID_2PC, data.timestamp + 8, MODIFIER_4PC));
            };
        }
    }
}
