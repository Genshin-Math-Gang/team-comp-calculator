using System;
using System.Collections.Generic;
using Tcc.elements;
using Tcc.events;
using Tcc.stats;
using Tcc.units;
using Tcc.weapons;

namespace Tcc.Units
{
    public class Albedo: Unit
    {
        SnapshottedStats burstSnapshot;
        private double lastIsotoma;
        public Albedo(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6):
            base("Albedo", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.GEO, WeaponType.SWORD)
        {
            
            this.burstSnapshot = new SnapshottedStats(this, Types.BURST);
            lastIsotoma = 0;
            AutoAttackFrameData = new[] {0, 12, 30, 59, 98, 152, 54};

        }  

        public override List<WorldEvent> Skill(double timestamp, params object[] p)
        {
            return new List<WorldEvent>
            {
                //Albedo MV for inital and placed skill are the same madge
                SkillActivated(timestamp),
                new Hit(timestamp+32/60f, 0, GetStatsPage, this, Types.SKILL,
                    new HitType(Element, heavy:true), "Solar Isotoma Place"),
                new WorldEvent(timestamp, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.DealDamageHook += SolarIsotoma;
                    }
                }),
                new WorldEvent(timestamp + 30, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.DealDamageHook  -= SolarIsotoma;
                    }
                })   
            };
        }

        public void SolarIsotoma(object? sender, DealDamageArgs args)
        {
            double timestamp = args.Timestamp;
            if (timestamp - lastIsotoma <= 1.99)
            {
                return;
            }
            double newIsotoma = timestamp;
            lastIsotoma = newIsotoma;
            args.World.AddWorldEvent(new Hit(newIsotoma, 1, GetStatsPage, this, Types.SKILL,
                new HitType(Element, heavy:true), "Solar Isotoma Hit"));
        }
 


        public override List<WorldEvent> Burst(double timestamp)
        {
            return new List<WorldEvent>
            {
                BurstActivated(timestamp),

               
                //scuffed needs work uwu how to snapshot E cast
            };
        }
 
   

 
        public override string ToString()
        {
            return "Albedo";
        }
    }
   
}
