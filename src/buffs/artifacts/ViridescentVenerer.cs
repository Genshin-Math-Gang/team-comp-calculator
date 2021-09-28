using System;
using System.Collections.Generic;
using Tcc.Stats;
using Tcc.Units;
using Tcc.Weapons;
using Tcc.Elements;

namespace Tcc.Buffs.Artifacts
{
    public class ViridescentVenerer: ArtifactSet
    {
        static readonly Guid ID_2PC = new("f35a16a0-2e30-489b-b3ae-4792234b50f8");
        static readonly Guid ID_4PC = new("e92a32e5-27b8-48f5-9ee3-b1179a67cf44");
        static readonly Guid HYDRO = new("2236a515-0a2f-45ab-838b-d6dc540d578f");
        static readonly Guid PYRO = new("8cb0d928-a503-48a4-a435-c82ef4088f80");
        static readonly Guid ELECTRO = new("0e18cfd8-a891-46ee-8e56-55a058e09270");
        static readonly Guid CRYO = new("761e2384-ed2b-45d3-b96a-21810b28bba6");
        private Dictionary<Reaction, (Guid, Element)> guidDict = new()
        {
            [Reaction.SWIRL_PYRO] = (PYRO, Element.PYRO),
            [Reaction.SWIRL_HYDRO] = (HYDRO, Element.HYDRO),
            [Reaction.SWIRL_ELECTRO] = (ELECTRO, Element.ELECTRO),
            [Reaction.SWIRL_CRYO] = (CRYO, Element.CRYO)
        };


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
            

            
            unit.swirlTriggeredHook += (_, param)
                => param.enemy.AddBuff(new RefreshableBuff<FirstPassModifier>(guidDict[param.reaction].Item1, param.timestamp + 10,
                    _ => new GeneralStats(elementalResistance: new KeyedPercentBonus<Element>(guidDict[param.reaction].Item2, -0.4))));
        }
        
    }
}