using System;
using Tcc.Elements;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs.Artifacts
{
    public class ThunderingFury: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("c8e4425b-699e-4bda-be53-cdaa6559d0f0");
        static readonly Guid ID_4PC = new Guid("50643482-51e8-414e-85b2-04da0f4e65ce");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Element.ELECTRO, 0.15);

        static readonly FirstPassModifier MODIFIER_4PC = (_) => new GeneralStats(reactionBonus: new KeyedPercentBonus<int>(
            (Reaction.OVERLOADED, 0.4),
            (Reaction.ELECTROCHARGED, 0.4),
            (Reaction.SUPERCONDUCT, 0.4)
        ));

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_4PC, MODIFIER_4PC));
    }
}
