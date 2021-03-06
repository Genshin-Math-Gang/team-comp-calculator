using System;
using Tcc.buffs;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class ShimenawasReminiscence: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("1643b232-91d2-4566-83d9-320bd7dde850");
        static readonly Guid ID_4PC = new Guid("15592ba5-acd1-47c1-a198-9189cfdfbd2a");

        static readonly FirstPassModifier MODIFIER_2PC = _ => (Stats.AtkPercent, 0.18);
        static readonly AbilityModifier MODIFIER_4PC = _ => (Stats.DamagePercent, 0.5);

        const int ENERGY_COST_4PC = 15;
        static readonly double COOLDOWN_4PC = (10);

        double cooldown4pcUntil;

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.SkillActivatedHook += (_, timestamp) =>
            {
                if(unit.CurrentEnergy < ENERGY_COST_4PC) return;

                if(cooldown4pcUntil != null && timestamp < cooldown4pcUntil) return;
                else cooldown4pcUntil = timestamp + COOLDOWN_4PC;

                unit.LoseEnergy(ENERGY_COST_4PC);

                unit.AddBuff(new RefreshableBuff<AbilityModifier>(
                    ID_4PC,
                    timestamp + COOLDOWN_4PC,
                    MODIFIER_4PC
                ), Types.NORMAL, Types.CHARGED, Types.PLUNGE);
            };
        }
    }
}
