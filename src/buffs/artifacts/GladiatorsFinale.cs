using System;
using Tcc.Stats;
using Tcc.Units;
using Tcc.Weapons;

namespace Tcc.Buffs.Artifacts
{
    public class GladiatorsFinale: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("99be7eb5-d65d-46ec-9da7-33edf59723f3");
        static readonly Guid ID_4PC = new Guid("db4c037b-71ff-4328-9bba-bfcbdf1ca01e");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => new GeneralStats(attackPercent: 0.18);
        static readonly GeneralStats STATS_4PC = new GeneralStats(damagePercent: 0.35);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        
        public override void Add4pc(World world, Unit unit)
        {
            unit.AddBuff(new PermanentBuff<AbilityModifier>(ID_4PC, (_) =>
            {
                switch (unit.Weapon.Type)
                {
                    case WeaponType.SWORD:
                    case WeaponType.CLAYMORE:
                    case WeaponType.POLEARM:
                        return STATS_4PC;
                    default:
                        return new AbilityStats();
                }
            }), Types.NORMAL);
        }
    }
}
