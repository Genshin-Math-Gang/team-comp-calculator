using System;
using Tcc.buffs;
using Tcc.events;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class Exile: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("2bf4f7b5-240a-4503-9b66-40d045f11670");
        static readonly Guid ID_4PC = new Guid("a0dc99c0-da62-41f1-a7f0-a5f320ac7ec8");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Stats.EnergyRecharge, 0.2);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.BurstActivatedHook += (_, timestamp) =>
            {
                var worldEvents = new WorldEvent[3];

                for(var tick = 0; tick < worldEvents.Length; tick++)
                {
                    worldEvents[tick] = new WorldEvent(timestamp + (tick + 1) * 2, (_) =>
                    {
                        foreach(var unitInParty in world.GetUnits())
                        {
                            if(unitInParty != unit) unitInParty.GiveEnergy(2);
                        }
                    });
                }
            };
        }
    }
}
