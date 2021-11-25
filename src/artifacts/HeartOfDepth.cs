using System;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class HeartOfDepth: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("3eac5874-0066-479d-aae7-3e1f8112bec6");
        static readonly Guid ID_4PC = new Guid("84b664c7-5851-4790-98ee-ad2af4817786");

        static readonly FirstPassModifier MODIFIER_2PC = _ => (Stats.Stats.HydroDamageBonus, 0.15);
        static readonly AbilityModifier MODIFIER_4PC = _ => (Stats.Stats.DamagePercent, 0.3);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.SkillActivatedHook += (_, timestamp)
                => unit.AddBuff(new RefreshableBuff<AbilityModifier>(ID_4PC, 
                    timestamp + 15, MODIFIER_4PC), Types.NORMAL, Types.CHARGED);
        }
    }
}
