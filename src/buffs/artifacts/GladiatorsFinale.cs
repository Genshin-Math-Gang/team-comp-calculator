using System;
using Tcc.Units;
using Tcc.Weapons;

namespace Tcc.Buffs.Artifacts
{
    public class GladiatorsFinale: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("99be7eb5-d65d-46ec-9da7-33edf59723f3");
        static readonly Guid ID_4PC = new Guid("db4c037b-71ff-4328-9bba-bfcbdf1ca01e");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(attackPercent: 0.18);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(damagePercent: 0.35);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicUnconditionalBuff(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit)
        {
            var weaponType = unit.Weapon.Type;

            if(weaponType == WeaponType.SWORD || weaponType == WeaponType.CLAYMORE || weaponType == WeaponType.POLEARM)
            {
                unit.AddBuff(new BasicUnconditionalBuff(ID_4PC, MODIFIER_4PC, Stats.Types.NORMAL));
            }
        }
    }
}
