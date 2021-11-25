using System;
using Tcc.buffs;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class MaidenBeloved: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("b7fd5f61-05f4-48c5-81ff-9663e99c1787");
        static readonly Guid ID_4PC = new Guid("aa8550f2-920f-4810-a2b3-81d7337092a6");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Stats.HealingBonus, 0.15);
        static readonly FirstPassModifier MODIFIER_4PC = (_) => (Stats.IncomingHealingBonus, 0.2);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            EventHandler<Timestamp> addBuff = (_, timestamp) =>
            {
                foreach(var unitInParty in world.GetUnits()) unitInParty.AddBuff(new RefreshableBuff<FirstPassModifier>(ID_4PC, timestamp + 10, MODIFIER_4PC));
            };

            unit.SkillActivatedHook += addBuff;
            unit.BurstActivatedHook += addBuff;
        }
    }
}
