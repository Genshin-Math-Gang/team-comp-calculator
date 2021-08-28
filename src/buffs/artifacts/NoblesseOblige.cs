using System;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class NoblessOblige: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("e1cb0bd0-46d0-4368-a316-e5f7b4435543");
        static readonly Guid ID_4PC = new Guid("e9ec234c-2100-4fa3-a775-8708b2a1161e");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(damagePercent: 0.2);
        static readonly Stats.Stats MODIFIER_4PC = new Stats.Stats(attackPercent: 0.2);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_2PC, MODIFIER_2PC, Stats.Types.BURST));

        public override void Add4pc(World world, Unit unit)
        {
            unit.burstActivatedHook += (_, timestamp) =>
            {
                foreach(var unitInParty in world.GetUnits()) unitInParty.AddBuff(new RefreshableBuff(ID_4PC, timestamp + 12, MODIFIER_4PC));
            };
        }
    }
}
