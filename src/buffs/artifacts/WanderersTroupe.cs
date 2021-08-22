using System;
using Tcc.Units;
using Tcc.Weapons;

namespace Tcc.Buffs.Artifacts
{
    public class WanderersTroupe: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("e111a06f-7f13-4dd7-8d08-cd33f2e1bd77");
        static readonly Guid ID_4PC = new Guid("5a976f3c-c777-4615-8b3a-6704401f4a83");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(elementalMastery: 80);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(damagePercent: 0.35);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicUnconditionalBuff(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            var weaponType = unit.Weapon.Type;

            if(weaponType == WeaponType.BOW || weaponType == WeaponType.CATALYST)
            {
                unit.AddBuff(new BasicUnconditionalBuff(ID_4PC, MODIFIER_4PC, Stats.Types.CHARGED));
            }
        }
    }
}
