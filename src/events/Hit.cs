using System;
using Tcc.buffs;
using Tcc.enemy;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events
{
    public class Hit: WorldEvent 
        // TODO: it might just be better to replace mvIndex with the MV itself
    {

        
        public Hit(
            double timestamp, double mvs, Func<double, SecondPassStatsPage> stats, Unit unit, Types type, 
            HitType hitType, string description = "", Enemy target = null, Buff<FirstPassModifier> debuff=null): 
            base(
                timestamp,

                // TODO We'll need to hook in reactions, enemies, multi-target and infusion here
                // TODO reaction is scuffed and only used for transformative reactions so i want to remove it
                world =>
                {
                    world.CalculateDamage(timestamp, mvs, stats(timestamp), unit, type, hitType, description);
                    if (debuff is null) return;
                    foreach (var enemy in world.Enemies)
                    {
                        enemy.AddBuff(debuff);
                    }

                }, description) { }


        
        public Hit(
            double timestamp, int mvIndex, Func<double, SecondPassStatsPage> stats, Unit unit, Types type, 
            HitType hitType, string description = "", Enemy target = null, Buff<FirstPassModifier> debuff=null): 
            base(
                timestamp,

                // TODO We'll need to hook in reactions, enemies, multi-target and infusion here
                // TODO reaction is scuffed and only used for transformative reactions so i want to remove it
                world =>
                {
                    world.CalculateDamage(timestamp, unit.StartingAbilityStats[type].GetMotionValue(mvIndex), 
                        stats(timestamp), unit, type, hitType, description);
                    if (debuff is null) return;
                    foreach (var enemy in world.Enemies)
                    {
                        enemy.AddBuff(debuff);
                    }
                }, description) { }
        

        /*public Hit(
            double timestamp, Func<Element> element, int mvIndex, Func<double, SecondPassStatsPage> stats,
            Unit unit, Types type, HitType hitType, 
            string description = "", Enemy target = null): 
            base(
                timestamp,

                // TODO We'll need to hook in reactions, enemies, multi-target and infusion here
                // TODO reaction is scuffed and only used for transformative reactions so i want to remove it
                (world) => world.CalculateDamage(
                    timestamp, element(), mvIndex, stats(timestamp), 
                    unit, type, hitType, description), description
            ) {
        }*/
    }
    
}