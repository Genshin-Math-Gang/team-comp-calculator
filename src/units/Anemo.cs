using System;
using Tcc.elements;
using Tcc.weapons;

namespace Tcc.units
{
    public abstract class Anemo: Unit
    {

        
        protected Element Infusion;
        protected InfusionRef InfusionRef;
        protected Anemo(string name, String level, int constellationLevel, int autoLevel, int skillLevel, int burstLevel, WeaponType weaponType): 
            base(name, level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.ANEMO, weaponType)
        {
            Infusion = Element.PHYSICAL;
            InfusionRef = new InfusionRef(() => Infusion, v => { Infusion = v; });
        }

        public Element GetInfusion() => InfusionRef.Value;

        public void SetInfusion(Element e) => InfusionRef.Value = e;

    }
}