using System;
using Tcc.Stats;
using Tcc.Units;
using Tcc.Weapons;
using Tcc.Elements;

namespace Tcc.Buffs.Artifacts
{
    public class ViridescentVenerer: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("f35a16a0-2e30-489b-b3ae-4792234b50f8");
        static readonly Guid ID_4PC = new Guid("e92a32e5-27b8-48f5-9ee3-b1179a67cf44");
        
        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Element.ANEMO, 0.15);
        static readonly FirstPassModifier MODIFIER_4PC_PASSIVE = (_) => new KeyedPercentBonus<Reaction>(
            (Reaction.SWIRL_CRYO, 0.6),
            (Reaction.SWIRL_PYRO, 0.6),
            (Reaction.SWIRL_HYDRO, 0.6),
            (Reaction.SWIRL_ELECTRO, 0.6)
        );
        
        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        
        public override void Add4pc(World world, Unit unit)
        {
            unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_4PC, MODIFIER_4PC_PASSIVE));

            /*unit.skillActivatedHook += (_, timestamp)
                => unit.AddBuff(new RefreshableBuff<FirstPassModifier>(ID_4PC_STACK, timestamp + 10, MODIFIER_4PC_STACK, maxStacks: 3));*/
        }
    }
}