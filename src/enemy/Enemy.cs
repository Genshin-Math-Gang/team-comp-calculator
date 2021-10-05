using System;
using System.Collections.Generic;
using System.Linq;
using Tcc.Buffs;
using Tcc.Stats;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Units;

namespace Tcc.Enemy
{
    public class Enemy: StatObject
    {

        private Gauge gauge;

        private Dictionary<Guid, ICD> icdDict;
        private readonly ICD noICD = new (new Timestamp(0), 0);
        private static Guid superconductID = new ("fb1fd9b8-6096-4dea-9e9a-5f3fa18976b8");

        private static GeneralStats superconductDebuff =
            new GeneralStats(elementalResistance: new KeyedPercentBonus<Element>(Element.PHYSICAL, -0.4));
        
        

        // dumb swirl hackery 
        private Dictionary<Reaction, int> swirlHitCounter;
        private Timestamp swirlLastChecked;

        public Enemy(GeneralStats stats=null, Gauge gauge=null, CapacityStats hp=null): base(stats, hp)
        {
            this.gauge = gauge ?? new Gauge();
            this.icdDict = new Dictionary<Guid, ICD>();
            this.swirlHitCounter = new Dictionary<Reaction, int>();
            this.swirlLastChecked = new Timestamp(0);
        }

        public Aura GetAura() => gauge.GetAura();
        
        public void AddBuff(Buff<FirstPassModifier> buff) => buff.AddToList(firstPassBuffs);
        
        
        // TODO: make transformative reactions not terrible tomorrow 
        public (double, List<WorldEvent>) TakeDamage(Timestamp timestamp, Element element, Types type,
            SecondPassStatsPage statsOfUnit, Unit unit, HitType hitType, int mvIndex = 0)
        {
            var unitAbilityStats = unit.GetAbilityStats(statsOfUnit, type, element, this, timestamp);
            ICD icd;
            ICDCreator creator = hitType.Creator;
            if (creator is null)
            {
                icd = noICD;
            }
            else if (icdDict.ContainsKey(creator.Guid))
            {
                icd = icdDict[creator.Guid];
            }
            else
            {
                ICD temp = creator.CreateICD();
                icdDict[creator.Guid] = temp;
                icd = temp;
            }

            var (reactionMultiplier, worldEvents) = gauge.ElementApplied(timestamp, element, hitType.Gauge,
                unit, statsOfUnit, type, icd, hitType.IsHeavy);
            List<WorldEvent> events = worldEvents ?? new List<WorldEvent>();
            Reaction reaction = hitType.ReactionType;
            double damage;
            if (type != Types.TRANSFORMATIVE)
            {
                damage = reactionMultiplier * 
                         unitAbilityStats.CalculateHitDamage(mvIndex, element) *
                         DefenceCalculator(timestamp, element, type, statsOfUnit) *
                         ResistanceCalculation(timestamp, element, type, statsOfUnit);
            }
            else // need a better way of handling transformative reactions
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

                    events.Add(unit.TriggeredSwirl(timestamp, reaction, this));
                }
                else if (reactionType == Reaction.SUPERCONDUCT)
                {
                    // superconduct resist shred
                    events.Add(new AddBuff<FirstPassModifier>(timestamp, 
                        new RefreshableBuff<FirstPassModifier>(superconductID, timestamp + 10, 
                            _ => superconductDebuff), this));
                }

                damage = TransformativeScaling.ReactionMultiplier(reaction) *
                         (TransformativeScaling.EmScaling(statsOfUnit.ElementalMastery) +
                          statsOfUnit.ReactionBonus.GetPercentBonus(reaction))
                         * TransformativeScaling.damage[statsOfUnit.Level] *
                         ResistanceCalculation(timestamp, element, type, statsOfUnit);
            }
            events.AddRange(TakeDamage(timestamp, damage));
            return (damage, events);
        }
        
        

        private double ResistanceCalculation (Timestamp timestamp, Element element, Types type, StatsPage statsOfUnit)
        {
            // idk if this is the correct function
            var resistance = GetFirstPassStats(timestamp).ElementalResistance[element];

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
            return (statsOfUnit.Level + 100d)/((1/* TODO - statsOfUnit.DEFReduction*/) * (startingGeneralStats.Level + 100) + statsOfUnit.Level + 100);
        }
        
        public bool HasAura(Aura aura) => throw new NotImplementedException();

        private List<WorldEvent> TakeDamage(Timestamp timestamp, double damage)
        {
            var events = new List<WorldEvent>();
            if (startingCapacityStats.Hp.ReduceValue(damage))
            {
                events.Add(new EnemyDeath(this, timestamp));
            }

            return events;
        }
    }

    
}