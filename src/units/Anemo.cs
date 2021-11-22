using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tcc.Buffs;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Weapons;

namespace Tcc.Units
{
    public abstract class Anemo: Unit
    {

        public Element Infusion = Element.PHYSICAL;
        protected Anemo(string name, String level, int constellationLevel, int autoLevel, int skillLevel, int burstLevel, WeaponType weaponType): 
            base(name, level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.ANEMO, weaponType)
        {
        }
        
        

        public void SetInfusion(Element e)
        {
            Infusion = e;
        }

        public Element GetInfusion()
        {
            return Infusion;
        }
    }
}