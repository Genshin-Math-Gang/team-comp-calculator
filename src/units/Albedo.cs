//im gay for insane
 
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tcc.events;
using Tcc.stats;
using Tcc.units;
using Tcc.elements;
using Tcc.weapons;

namespace Tcc.Units
{
    public class Albedo: Unit
    {
        SnapshottedStats burstSnapshot;
        public Albedo(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6):
            base("Albedo", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.GEO, WeaponType.SWORD)
        {
            //cock
            this.burstSnapshot = new SnapshottedStats(this, Types.BURST);
           
        }  
        
 
        public override string ToString()
        {
            return "Albedo";
        }
    }
   
}
