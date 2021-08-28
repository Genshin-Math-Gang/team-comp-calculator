using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class RetracingBolide: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("cccbadd8-9df1-4b4a-b5a5-2e5e7c2af191");
        static readonly Guid ID_4PC = new Guid("9574a114-a0b4-442c-8c4b-828eff8a34ad");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(shieldStrength: 0.35);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(damagePercent: 0.4);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            unit.AddBuff(new BasicBuffFromStats(ID_4PC, MODIFIER_4PC, Stats.Types.NORMAL | Stats.Types.CHARGED, (_, _, _) => unit.IsShielded));
        }
    }
}
