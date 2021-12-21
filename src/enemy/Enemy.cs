using System;
using System.Collections.Generic;
using Tcc.buffs;
using Tcc.elements;
using Tcc.events;
using Tcc.stats;
using Tcc.units;

namespace Tcc.enemy
{
    public class Enemy: StatObject
    {

        private Gauge gauge;

        private Dictionary<Guid, ICD> icdDict;
        private readonly ICD noICD = new (new Timestamp(0), 0);
        
        private static Guid superconductID = new ("fb1fd9b8-6096-4dea-9e9a-5f3fa18976b8");
        private static StatsPage superconductDebuff =
            new StatsPage(Stats.PhysicalResistance, -0.4);
        
        // dumb swirl hackery 
        private Dictionary<Reaction, int> swirlHitCounter;
        private Timestamp swirlLastChecked;
        

        public Enemy(StatsPage statsPage = null, Gauge gauge = null) : base(statsPage ?? new StatsPage(Stats.HpBase, 100000))
        {
            this.gauge = gauge ?? new Gauge();
            icdDict = new Dictionary<Guid, ICD>();
            swirlHitCounter = new Dictionary<Reaction, int>();
            swirlLastChecked = new Timestamp(0);
        }

        public Aura GetAura() => gauge.GetAura();
        
        
        // TODO: make transformative reactions not terrible tomorrow 
        public (double, List<WorldEvent>) TakeDamage(Timestamp timestamp, Types type,
            SecondPassStatsPage statsOfUnit, Unit unit, HitType hitType, int index)
        {
            Element element = hitType.Element;
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
                         unitAbilityStats.CalculateHitMultiplier(index, element) *
                         DefenceCalculator(timestamp, unit) *
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
                         (TransformativeScaling.EmScaling(statsOfUnit[Stats.ElementalMastery]) +
                          statsOfUnit.ReactionBonus(reaction))
                         * TransformativeScaling.damage[unit.Level] *
                         ResistanceCalculation(timestamp, element, type, statsOfUnit);
            }
            events.AddRange(TakeDamage(timestamp, damage));
            return (damage, events);
        }
        
        

        private double ResistanceCalculation (Timestamp timestamp, Element element, Types type, StatsPage statsOfUnit)
        {
            // idk if this is the correct function
            var resistance = GetFirstPassStats(timestamp)[Converter.ElementToRes(element)];

            return resistance switch
            {
                < 0 => 1 - resistance / 2,
                < 0.75 => 1 - resistance,
                _ => 1.0 / (4 * resistance + 1)
            };
        }

        private double DefenceCalculator (Timestamp timestamp, Unit unit)
        {
            return (unit.Level + 100d)/((1/* TODO - statsOfUnit.DEFReduction*/) * (Level + 100) + unit.Level + 100);
        }
        
        public bool HasAura(Aura aura) => throw new NotImplementedException();

        private List<WorldEvent> TakeDamage(Timestamp timestamp, double damage)
        {
            var events = new List<WorldEvent>();
            CurrentHp -= damage;
            if (CurrentHp == 0) events.Add(new EnemyDeath(this, timestamp));
            return events;
        }
    }

    
}