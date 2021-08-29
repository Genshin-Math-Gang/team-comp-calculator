using System.Collections.Generic;
using System.Linq;
using System;
using Tcc.Elements;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class DefendersWill: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("4585f5b3-f405-4b1c-8c19-8fdac30c17bc");
        static readonly Guid ID_4PC = new Guid("96d7c6e1-22dd-4045-8554-8035ebabf493");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => new GeneralStats(defencePercent: 0.3);

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            var resistances = world.GetUnits()
                .Select((unit) => (unit.element, 0.3))
                .Distinct()
                .ToArray();

            var elementalResistance = new KeyedPercentBonus<Element>(resistances);

            unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_4PC, (_) => new GeneralStats(elementalResistance: elementalResistance)));
        }
    }
}
