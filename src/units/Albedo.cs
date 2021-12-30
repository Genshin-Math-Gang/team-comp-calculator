using System;
using System.Collections.Generic;
using Tcc.elements;
using Tcc.events;
using Tcc.stats;
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
            
            this.burstSnapshot = new SnapshottedStats(this, Types.BURST);
           
        }  

        public List<WorldEvent> Skill(Timestamp timestamp)
        {
            return new List<WorldEvent>
            {
                //Albedo MV for inital and placed skill are the same madge
                SKillActivated(timestamp),
                new Hit(timestamp+32/60f, element, GetStatsPage, this, Types.SKILL,
                    new HitType(heavy:true), "Solar Isotoma Place"),
                new WorldEvent(timestamp, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.DoDamageHook += SolarIsotoma;
                    }
                }),
                new WorldEvent(timestamp + 30, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.DoDamageHook -= SolarIsotoma;
                    }
                })   
            };
        }

        public void SolarIsotoma(object? sender, Timestamp t)
        {
            
            if (timestamp - lastIsotoma <= 1.99)
            {
                return;
            }
            newIsotoma = t;
            lastIsotoma = newIsotoma;
            t.World.AddWorldEvent(new Hit(newIsotoma, element, GetStatsPage, this, Type.SKILL,
                new HitType(heavy:true), "Solar Isotoma Hit"));
        }
 

/*
        public List<WorldEvent> Burst(Timestamp timestamp)
        {
            return new List<WorldEvent>
            {
                BurstActivated(timestamp),
                burstSnapshot.Snapshot(timestamp),
                new Hit(timestamp+96/60f, element, GetStatsPage, this, Types.BURST,
                new HitType(heavy:true), "Rite of Progeniture: Tectonic Tide"),
                //find fatal blossom frame
                for (int i = 0; i < 7; i++){
                    new Hit(timestamp+96/60+i/6f, element, GetStatsPage, this, Types.BURST,
                        new HitType(heavy:true), "Fatal Blossom")
                }
               
                //scuffed needs work uwu how to snapshot E cast
            };
        }
 
   */

 
        public override string ToString()
        {
            return "Albedo";
        }
    }
   
}
