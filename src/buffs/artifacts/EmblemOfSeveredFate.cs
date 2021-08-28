using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class EmblemOfSeveredFate: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("7d7cac85-39d8-4e72-994f-61a82f7a1c12");
        static readonly Guid ID_4PC = new Guid("61d31a59-db6b-42b7-a139-25294f40d9b0");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(energyRecharge: 0.2);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.AddBuff(new BasicBuffFromStats(
                ID_4PC,
                (_, unconditionalStats, _) => new Stats.Stats(damagePercent: Math.Min(unconditionalStats.EnergyRecharge * 0.25, 0.75)),
                Stats.Types.BURST
            ));
        }
    }
}
