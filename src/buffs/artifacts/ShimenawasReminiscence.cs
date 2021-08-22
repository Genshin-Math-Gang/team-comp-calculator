using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class ShimenawasReminiscence: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("1643b232-91d2-4566-83d9-320bd7dde850");
        static readonly Guid ID_4PC = new Guid("15592ba5-acd1-47c1-a198-9189cfdfbd2a");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(attackPercent: 0.18);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(damagePercent: 0.5);

        const int ENERGY_COST_4PC = 15;
        static readonly Timestamp COOLDOWN_4PC = new Timestamp(10);

        Timestamp cooldown4pcUntil;

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicUnconditionalBuff(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.skillActivatedHook += (_, timestamp) =>
            {
                if(unit.CurrentEnergy < ENERGY_COST_4PC) return;

                if(cooldown4pcUntil != null && timestamp < cooldown4pcUntil) return;
                else cooldown4pcUntil = timestamp + COOLDOWN_4PC;

                unit.LoseEnergy(ENERGY_COST_4PC);

                unit.AddBuff(new RefreshableBuff(
                    ID_4PC,
                    timestamp + COOLDOWN_4PC,
                    MODIFIER_4PC,
                    Stats.Types.NORMAL | Stats.Types.CHARGED | Stats.Types.PLUNGE
                ));
            };
        }
    }
}
