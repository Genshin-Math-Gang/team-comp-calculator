using System;
using Tcc.Stats;
using Tcc.Units;
using Tcc.Weapons;

namespace Tcc.Buffs.Artifacts
{
    public class WanderersTroupe: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("e111a06f-7f13-4dd7-8d08-cd33f2e1bd77");
        static readonly Guid ID_4PC = new Guid("5a976f3c-c777-4615-8b3a-6704401f4a83");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => new GeneralStats(elementalMastery: 80);
        static readonly GeneralStats STATS_4PC = new GeneralStats(damagePercent: 0.35);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.AddBuff(new PermanentBuff<AbilityModifier>(ID_4PC, (_) =>
            {
                switch(unit.weaponType)
                {
                    case WeaponType.BOW:
                    case WeaponType.CATALYST:
                        return STATS_4PC;
                    default:
                        return new AbilityStats();
                }
            }), Types.CHARGED);
        }
    }
}
