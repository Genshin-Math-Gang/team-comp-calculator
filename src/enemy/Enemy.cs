using System;
using Tcc.Stats;
using Tcc.Elements;
using Tcc.Units;

namespace Tcc.Enemy
{
    public class Enemy
    {
        private Stats.Stats stats;
        public Gauge gauge;

        public Enemy (Stats.Stats stats = null, Gauge gauge = null)
        {
            this.stats = stats == null ? new Stats.Stats() : stats;
            this.gauge = gauge == null ? new Gauge() : gauge;
        }

        public double takeDamage (Timestamp timestamp, Element element, Types type, Stats.Stats stats_of_unit, Unit unit, int mvIndex, double reaction = Reaction.NONE, bool? isHeavy = false)
        {
            Stats.Stats result = unit.AddStatsFromEnemy(stats_of_unit, type, this, timestamp);
            
            if (type != Types.TRANSFORMATIVE)
            {
                return CalculateHitDamage(mvIndex, element, result) * DefenceCalculator(timestamp, element, type, stats_of_unit) * ResistanceCalculation(timestamp, element, type, stats_of_unit);
            }
            else
            {
                return  Reaction.ReactionMultiplier(reaction) * 16 * (1 + (stats.ElementalMastery/(stats.ElementalMastery + 2000)) + stats.ReactionBonus.GetDamagePercentBonus(reaction)) * TransformativeScaling.damage[stats.Level] * ResistanceCalculation(timestamp, element, type, stats_of_unit);
            }
        }
        private double CalculateHitDamage(int mvIndex, Element element, Stats.Stats stats)
        {
            var totalDamagePercent = 1 + stats.DamagePercent;

            totalDamagePercent += stats.ElementalBonus.GetDamagePercentBonus(element);

            return stats.MotionValues[mvIndex] * stats.Attack * totalDamagePercent * (1 + stats.CritRate * stats.CritDamage);
        }

        private double ResistanceCalculation (Timestamp timestamp, Element element, Types type, Stats.Stats stats_of_unit)
        {
            if (stats.ElementalResistance.GetDamagePercentBonus(element) < 0)
            {
                return  1 - stats.ElementalResistance.GetDamagePercentBonus(element)/2;
            }
            if (stats.ElementalResistance.GetDamagePercentBonus(element) < 0.75)
            {
                return 1 - stats.ElementalResistance.GetDamagePercentBonus(element);
            }

            return 1/(4*stats.ElementalResistance.GetDamagePercentBonus(element) + 1);
        }

        private double DefenceCalculator (Timestamp timestamp, Element element, Types type, Stats.Stats stats_of_unit)
        {
            return (stats_of_unit.Level + 100)/((1 - stats_of_unit.DEFReduction) * (stats.Level + 100) + stats_of_unit.Level + 100);
        }
        
        public bool HasAura(Aura aura) => throw new NotImplementedException();
    }

    
}