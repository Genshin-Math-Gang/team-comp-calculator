using System;
using Tcc.Stats;
using Tcc.Units;
using Tcc.Weapons;

namespace Tcc.Buffs.Artifacts
{
    public class Scholar: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("7d94df2c-995e-4f83-9daa-57d3b497d594");
        static readonly Guid ID_4PC = new Guid("6d44e17e-6ebe-490b-b76e-da5fe68e1f0b");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Stats.Stats.EnergyRecharge, 0.2);

        Timestamp cooldown4pcUntil;

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.ParticleCollectedHook += (_, data) =>
            {
                if(cooldown4pcUntil != null && data.timestamp < cooldown4pcUntil) return;
                else cooldown4pcUntil = data.timestamp + 3;

                foreach(var unitInParty in world.GetUnits())
                {
                    var weaponType = unitInParty.Weapon.Type;

                    if(weaponType == WeaponType.BOW || weaponType == WeaponType.CATALYST) unitInParty.GiveEnergy(3);
                }
            };
        }
    }
}
