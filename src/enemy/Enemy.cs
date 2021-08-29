using System;
using Tcc.Stats;
using Tcc.Elements;
using Tcc.Units;

namespace Tcc.Enemy
{
    public class Enemy
    {
        private GeneralStats stats;
        public Gauge gauge;

        public Enemy(GeneralStats stats = null, Gauge gauge = null)
        {
            this.stats = stats ?? new GeneralStats();
            this.gauge = gauge ?? new Gauge();
        }

        public double takeDamage (Timestamp timestamp, Element element, Types type, StatsPage stats_of_unit, Unit unit, int mvIndex, double reaction = Reaction.NONE, bool? isHeavy = false)
        {
            var unitAbilityStats = unit.GetAbilityStats(stats_of_unit, type, this, timestamp);
            
            if (type != Types.TRANSFORMATIVE)
            {
                return unitAbilityStats.CalculateHitDamage(mvIndex, element) * DefenceCalculator(timestamp, element, type, stats_of_unit) * ResistanceCalculation(timestamp, element, type, stats_of_unit);
            }
            else
            {
                return Reaction.ReactionMultiplier(reaction) * 16 * (1 + (stats.ElementalMastery/(stats.ElementalMastery + 2000)) + stats.ReactionBonus.GetPercentBonus(reaction)) * TransformativeScaling.damage[stats.Level] * ResistanceCalculation(timestamp, element, type, stats_of_unit);
            }
        }

        private double ResistanceCalculation (Timestamp timestamp, Element element, Types type, StatsPage stats_of_unit)
        {
            var resistance = stats.ElementalResistance.GetPercentBonus(element);

            if (resistance < 0)
            {
                return  1 - resistance / 2;
            }
            if (resistance < 0.75)
            {
                return 1 - resistance;
            }

            return 1/(4 * resistance + 1);
        }

        private double DefenceCalculator (Timestamp timestamp, Element element, Types type, StatsPage stats_of_unit)
        {
            return (stats_of_unit.Level + 100)/((1 - stats_of_unit.DEFReduction) * (stats.Level + 100) + stats_of_unit.Level + 100);
        }
        
        public bool HasAura(Aura aura) => throw new NotImplementedException();
    }

    
}