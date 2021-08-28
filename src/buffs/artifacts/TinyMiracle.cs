using System.Linq;
using System;
using Tcc.Elements;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class TinyMiracle: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("6a9457be-248c-41d3-b59f-f6b77d78a5d5");

        static readonly Stats.Stats MODIFIER_2PC = new Stats.Stats(elementalResistance: new KeyedPercentBonus<Element>(
            Enum.GetValues<Element>()
                .Select((element) => (element, 0.2))
                .ToArray()
        ));

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new BasicBuffFromUnit(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => throw new NotImplementedException();
    }
}
