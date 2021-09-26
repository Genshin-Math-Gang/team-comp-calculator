using System;
using System.Collections.Generic;
using Tcc.Stats;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Units;

namespace Tcc.Enemy
{
    public class Enemy
    {
        private GeneralStats stats;
        public Gauge gauge;
        private Dictionary<Guid, ICD> icdDict;
        private readonly ICD noICD = new (new Timestamp(0), 0);
        // dumb swirl hackery 
        private Dictionary<Reaction, int> swirlHitCounter;
        private Timestamp swirlLastChecked;

        public Enemy(GeneralStats stats = null, Gauge gauge = null)
        {
            this.stats = stats ?? new GeneralStats();
            this.gauge = gauge ?? new Gauge();
            this.icdDict = new Dictionary<Guid, ICD>();
            this.swirlHitCounter = new Dictionary<Reaction, int>();
            this.swirlLastChecked = new Timestamp(0);
        }
        
        
        // TODO: make transformative reactions not terrible tomorrow 
        public (double, List<WorldEvent>) TakeDamage (Timestamp timestamp, Element element, Types type, 
            SecondPassStatsPage statsOfUnit, Unit unit, HitType hitType, int mvIndex = 0)
        {
            var unitAbilityStats = unit.GetAbilityStats(statsOfUnit, type, element, this, timestamp);
            ICD icd;
            ICDCreator creator = hitType.Creator;
            if (creator is null)
            {
                icd = noICD;
            } else if (icdDict.ContainsKey(creator.Guid))
            {
                icd = icdDict[creator.Guid];
            } else
            {
                ICD temp = creator.CreateICD();
                icdDict[creator.Guid] = temp;
                icd = temp;
            }

            var results = this.gauge.ElementApplied(timestamp, element, unit.GetAbilityGauge(type),
                unit, statsOfUnit, type, icd, hitType.IsHeavy);
            double reactionMultiplier = results.Item1;
            List<WorldEvent> events = results.Item2;
            Reaction reaction = hitType.ReactionType;
            if (type != Types.TRANSFORMATIVE)
            {
                return (reactionMultiplier * unitAbilityStats.CalculateHitDamage(mvIndex, element) * 
                       DefenceCalculator(timestamp, element, type, statsOfUnit) * 
                       ResistanceCalculation(timestamp, element, type, statsOfUnit), events);
            }
            else// need a better way of handling transformative reactions
            {
                var reactionType = hitType.ReactionType;
                if (ReactionTypes.IsSwirl(reactionType))
                {
                    if (timestamp != swirlLastChecked)
                    {
                        // reset dictionary if time is different from last occurence of swirl hit
                        swirlLastChecked = timestamp;
                        swirlHitCounter = new Dictionary<Reaction, int>();
                    }

                    // add reaction to dict if it doesn't exist or increment if it does
                    if (!swirlHitCounter.TryAdd(reactionType, 1))
                    {
                        swirlHitCounter[reactionType] += 1;
                    }

                    if (swirlHitCounter[reactionType] > 2)
                    {
                        return (-1, null);
                    }
                }
                return (TransformativeScaling.ReactionMultiplier(reaction) * 
                       (TransformativeScaling.EmScaling(statsOfUnit.ElementalMastery) + 
                        statsOfUnit.ReactionBonus.GetPercentBonus(reaction))
                       * TransformativeScaling.damage[statsOfUnit.Level] * 
                       ResistanceCalculation(timestamp, element, type, statsOfUnit), null);
            }
        }

        private double ResistanceCalculation (Timestamp timestamp, Element element, Types type, StatsPage statsOfUnit)
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

        private double DefenceCalculator (Timestamp timestamp, Element element, Types type, StatsPage statsOfUnit)
        {
            return (statsOfUnit.Level + 100d)/((1/* TODO - statsOfUnit.DEFReduction*/) * (stats.Level + 100) + statsOfUnit.Level + 100);
        }
        
        public bool HasAura(Aura aura) => throw new NotImplementedException();
    }

    
}