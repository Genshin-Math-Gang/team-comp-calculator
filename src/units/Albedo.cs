//im gay for insane
 
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Weapons;
 
namespace Tcc.Units
{
    public class Albedo: Unit
    {
        SnapshottedStats burstSnapshot;
        public Albedo(int constellationLevel=0, string level="90", int autolevel=6, into skillLevel=6, int burstLevel=6):
            base("Albedo", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.GEO, WeaponType.SWORD)
        {
            //cock
            this.burstSnapshot = new SnapshottedStats(this, Types.BURST);
           
        }  
 
        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            return new List<WorldEvent>
            {
                //Albedo MV for inital and placed skill are the same madge
                SKillActivated(timestamp),
                new Hit(timestamp+32/60, element, GetStatsPage, this, Types.SKILL,
                new HitType(heavy:true), "Solar Isotoma Place"),
                for(int i = 0, i < 15, i++){
                    new Hit(timestamp+32/60+120*i, element, GetStatsPage, this. Types.SKILL,
                    new HitType(heavy:true, "Solar Isotoma"))
                }
            };
        }
 
        public List<WorldEvent> Burst(Timestamp timestamp)
        {
            return new List<WorldEvent>
            {
                BurstActivated(timestamp),
                burstSnapshot.Snapshot(timestamp),
                new Hit(timestamp+96/60, element, GetStatsPage, this, Types.BURST,
                new HitType(heavy:true), "Rite of Progeniture: Tectonic Tide"),
                //find fatal blossom frame
                for (int i = 0, i < 7, i++){
                    new Hit(timestamp+96/60+i/6, element, GetStatsPage, this, Types.BURST,
                        new HitType(heavy:true), "Fatal Blossom")
                }
               
                //scuffed needs work uwu how to snapshot E cast
            };
        }
 
   
 
        public override string ToString()
        {
            return "Albedo";
        }
    }
   
}
