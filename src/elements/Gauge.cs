using System;
using System.Collections.Generic;
using Tcc.events;
using Tcc.events.reactions;
using Tcc.stats;
using Tcc.units;

namespace Tcc.elements
{
    
    // electro and hydro can coexist, frozen is weird 
    // for now i'm just going to assume valid constructions are given
    public class Gauge
    {        
        private Dictionary<Element, GaugeElement> gaugeDict = new Dictionary<Element, GaugeElement>();
        private double LastChecked = 0;
        private Aura aura = Aura.NONE;
        private double FreezeDuration = 0;
        private double FreezeAura = 0;

        public Gauge() {}

        public Aura GetAura() => aura;

        public void Reset()
        {
            LastChecked = 0;
            aura = Aura.NONE;
            gaugeDict.Clear();
            FreezeAura = 0;
            FreezeAura = 0;
        }

        private static double MultiplicativeMultiplier (StatsPage stats, Reaction reaction)
        {
            double em = stats[Stats.ElementalMastery];
            double bonus = stats.ReactionBonus(reaction);
            return 1 + 2.78 * em / (1400 + em) + bonus;
        }
        
        public (double, List<WorldEvent>) ElementApplied(double timestamp, Element elementType, double gaugeStrength, 
            Unit unit, SecondPassStatsPage statsPage, Types type, ICD icd, bool isHeavy=false)
        {

            // I'm unclear what this is for
            if (type == Types.TRANSFORMATIVE) return (1, null);  

            // what is this
            //Tuple<Unit, Types> key = new Tuple<Unit, Types>(unit, type);
            
            
            List<WorldEvent> transformativeReactions = TimeDecay(timestamp, statsPage, unit);
            LastChecked = timestamp;
            
            if (!icd.checkICD(timestamp))
            {
                return (1, transformativeReactions);
            } 
            if (gaugeStrength == 0)
            {
                return (1, null);
            }
            
            if (elementType == Element.PHYSICAL)
            {
                return (1, null);
            } 
            
            

            var strength = gaugeStrength * 1.25;
            // need to do something else regarding damage but for now i just want to track aura properly
            // swirl is terrifying 
            // frozen is weird
            
            double multiplier = 1;
            if (isHeavy && aura == Aura.FROZEN)
            {
                transformativeReactions.Add(new Shatter(timestamp, statsPage, unit));
                RemoveFrozen();
            }

            switch (aura)
            {
                case Aura.NONE:
                    if (elementType is Element.GEO or Element.ANEMO)
                    {
                        return (1, null);
                    }
                    gaugeDict.Add(elementType, new GaugeElement(elementType, gaugeStrength)); 
                    aura = Converter.ElementToAura(elementType);
                    return (1, null);
                case Aura.PYRO:
                    switch (elementType)
                    {
                        case Element.PYRO:
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
                            return (1, null);
                        case Element.HYDRO:
                            strength *= 2;
                            multiplier = 2 * MultiplicativeMultiplier (statsPage, Reaction.VAPORIZE);
                            break;
                        case Element.CRYO:
                            strength /= 2;
                            multiplier =  1.5 * MultiplicativeMultiplier (statsPage, Reaction.MELT);
                            break;
                        case Element.ELECTRO:
                            transformativeReactions.Add(new Overload(timestamp, statsPage, unit));
                            break;
                        case Element.ANEMO:
                            transformativeReactions.Add(new Swirl(timestamp, statsPage, unit,Element.PYRO));
                            strength /= 2;
                            break;
                        case Element.GEO:
                            strength /= 2;
                            break;
                    }
                    DecreaseElement(Element.PYRO, strength);
                    return (multiplier, transformativeReactions);
                case Aura.HYDRO:
                    switch (elementType)
                    {
                        case Element.HYDRO:
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
                            return (1, null);
                        case Element.PYRO:
                            strength /= 2;
                            multiplier =  1.5 * MultiplicativeMultiplier (statsPage, Reaction.VAPORIZE);
                            break;
                        case Element.CRYO:
                            SetFrozen(gaugeDict[Element.HYDRO].GaugeValue, strength);
                            break;
                        case Element.ELECTRO:
                            aura = Aura.ELECTROCHARGED;
                            gaugeDict[elementType] = new GaugeElement(elementType, strength);
                            transformativeReactions.Add(new ElectroCharged(timestamp, statsPage, unit));
                            break;
                        case Element.ANEMO:
                            transformativeReactions.Add(new Swirl(timestamp, statsPage, unit,Element.HYDRO));
                            strength /= 2;
                            break;
                        case Element.GEO:
                            strength /= 2;
                            break;
                    }
                    DecreaseElement(Element.HYDRO, strength);
                    return (multiplier, transformativeReactions);
                case Aura.CRYO:
                    switch (elementType)
                    {
                        
                        case Element.CRYO:
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
                            return (1, null);
                        case Element.PYRO:
                            strength *= 2;
                            multiplier =  2 * MultiplicativeMultiplier (statsPage, Reaction.MELT);
                            break;
                        case Element.HYDRO:
                            SetFrozen(gaugeDict[Element.CRYO].GaugeValue, strength);
                            break;
                        case Element.ELECTRO:
                            transformativeReactions.Add(new Superconduct(timestamp, statsPage, unit));
                            break;
                        case Element.ANEMO:
                            transformativeReactions.Add(new Swirl(timestamp, statsPage, unit,Element.CRYO));
                            strength /= 2;
                            break;
                        case Element.GEO:
                            strength /= 2;
                            break;
                    }
                    DecreaseElement(Element.CRYO, strength);
                    return (multiplier, transformativeReactions);
                case Aura.ELECTRO:
                    switch (elementType)
                    {
                        
                        case Element.ELECTRO:
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
                            return (1, null);
                        case Element.PYRO:
                            transformativeReactions.Add(new Overload(timestamp, statsPage, unit));
                            break;
                        case Element.HYDRO:
                            aura = Aura.ELECTROCHARGED;
                            gaugeDict[elementType] = new GaugeElement(elementType, strength);
                            break;
                        case Element.CRYO:
                            transformativeReactions.Add(new Superconduct(timestamp, statsPage, unit));
                            break;
                        case Element.ANEMO:
                            transformativeReactions.Add(new Swirl(timestamp, statsPage, unit,Element.ELECTRO));
                            strength /= 2;
                            break;
                        case Element.GEO:
                            strength /= 2;
                            break;
                    }
                    DecreaseElement(Element.ELECTRO, strength);
                    return (multiplier, transformativeReactions);
                case Aura.ELECTROCHARGED:
                    switch (elementType)
                    {
                        case Element.PYRO:
                            DecreaseElement(Element.HYDRO, 2 * strength);
                            DecreaseElement(Element.ELECTRO, strength);
                            multiplier = 1.5 * MultiplicativeMultiplier (statsPage, Reaction.VAPORIZE);
                            strength = 0;
                            break;
                        case Element.HYDRO:
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
                            strength = 0;
                            break;
                        case Element.CRYO:
                            // TODO: i'm not actually sure what happens when you apply cryo to an ec enemy
                            break;
                        case Element.ELECTRO:
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
                            strength = 0;
                            break;
                        case Element.ANEMO:
                            strength /= 2;
                            if (gaugeDict[Element.ELECTRO].GaugeValue > strength)
                            {
                                transformativeReactions.Add(new Swirl(timestamp, statsPage, unit,Element.ELECTRO));
                                DecreaseElement(Element.ELECTRO, strength);
                            }
                            else
                            {
                                // is there an order in which these occur in game
                                transformativeReactions.Add(new Swirl(timestamp, statsPage, unit,Element.HYDRO));
                                transformativeReactions.Add(new Swirl(timestamp, statsPage, unit,Element.ELECTRO));
                                DecreaseElement(Element.ELECTRO, strength);
                                DecreaseElement(Element.HYDRO, strength);
                            }
                            strength = 0;
                            break;
                        case Element.GEO:
                            // always reacts with electro
                            DecreaseElement(Element.ELECTRO, strength / 2);
                            strength = 0;
                            break;
                    }
                    /* DecreaseElement(Element.HYDRO, strength);
                    DecreaseElement(Element.ELECTRO, strength); */
                    return (multiplier, transformativeReactions);
                case Aura.FROZEN:
                    // frozen may be a bit inaccurate because it is very convoluted 
                    switch (elementType)
                    {
                        case Element.PYRO:
                            strength *= 2;
                            multiplier = 2 * MultiplicativeMultiplier (statsPage, Reaction.MELT);
                            break;
                        case Element.HYDRO:
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
                            strength = 0;
                            break;
                        case Element.CRYO:
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
                            strength = 0;
                            break;
                        case Element.ELECTRO:
                            transformativeReactions.Add(new Superconduct(timestamp, statsPage, unit));
                            break;
                        case Element.ANEMO:
                            strength /= 2;
                            if (!gaugeDict.ContainsKey(Element.HYDRO))
                            {
                                transformativeReactions.Add(new Swirl(timestamp, statsPage, unit,Element.CRYO));
                            }
                            else if (gaugeDict[Element.HYDRO].GaugeValue > strength)
                            {
                                transformativeReactions.Add(new Swirl(timestamp, statsPage, unit,Element.HYDRO));
                                DecreaseElement(Element.HYDRO, strength);
                            }
                            else
                            {
                                transformativeReactions.Add(new Swirl(timestamp, statsPage, unit,Element.HYDRO));
                                transformativeReactions.Add(new Swirl(timestamp, statsPage, unit,Element.CRYO));
                                DecreaseElement(Element.HYDRO, strength);
                            }

                            strength = 0;
                            break;
                        case Element.GEO:
                            Console.WriteLine("crystallize shouldn't be able to occur on frozen enemy");
                            break;
                    }
                    DecreaseFrozen(strength);
                    if (gaugeDict.ContainsKey(Element.CRYO))
                    {
                        // if a frozen enemy has an underlying cryo aura that also has gauge consumed on reaction
                        DecreaseElement(Element.CRYO, strength);
                    }

                    return (multiplier, transformativeReactions);
            }
            Console.WriteLine("Something weird happened");
            return (1, transformativeReactions);
        }

        private List<WorldEvent> TimeDecay(double timestamp, SecondPassStatsPage statsPage, Unit unit)
        {
            if (timestamp < LastChecked)
            {
                throw new ArgumentException("Time input before last checked, cannot go back in time");
            }

            double timeSince = timestamp - LastChecked;
            LastChecked = timestamp;
            List<WorldEvent> ecTicks = new List<WorldEvent>();
            
            // this is pretty scuffed but it should work
            // also i'm just ignoring how hit lag can change EC slightly because that is a mess
            if (aura == Aura.ELECTROCHARGED)
            {
                double timer = (0);
                while (timer < timeSince)
                {
                    timer += 1;
                    gaugeDict[Element.ELECTRO].TimeDecay((1));
                    gaugeDict[Element.HYDRO].TimeDecay((1));
                    DecreaseElement(Element.ELECTRO, 0.4);
                    DecreaseElement(Element.HYDRO, 0.4);
                    // trigger EC here
                    // TODO: make electrocharged reactions happen at different times and think about how swirl can mess it up
                    ecTicks.Add(new ElectroCharged(timestamp, statsPage, unit));
                    if (aura != Aura.ELECTROCHARGED)
                    {
                        timeSince -= timer;
                        // kinda scuffed
                        goto Time;
                    }

                }
                timeSince -= timer;
                if (timeSince < 0.5)
                {
                    goto Time;
                }
                gaugeDict[Element.ELECTRO].TimeDecay(timeSince);
                gaugeDict[Element.HYDRO].TimeDecay(timeSince);
                if (gaugeDict[Element.ELECTRO].GaugeValue == 0 || gaugeDict[Element.HYDRO].GaugeValue == 0)
                {
                    // trigger EC here
                }
            
            }
            if (aura == Aura.FROZEN)
            {
                FreezeDuration = (Math.Min(0, FreezeDuration - timeSince));
                if (FreezeDuration == 0)
                {
                    RemoveFrozen();
                }
            }
            Time:
            foreach (var pair in gaugeDict)
            {
                    Element element = pair.Key;
                    GaugeElement gaugeElement = pair.Value;
                    //element.TimeDecay(timeSince);
                    ElementTimeDecay(element, timeSince);
            }

            return ecTicks;
        }

        private void ElementTimeDecay(Element element, double timeSince)
        {
            GaugeElement gaugeElement = gaugeDict[element];
            double decay = timeSince / gaugeElement.DecayRate;
            DecreaseElement(element, decay);
        }
        private void DecreaseElement(Element element, double value)
        {
            GaugeElement gauge = gaugeDict[element];
            double current = gauge.GaugeValue;
            if (current < value)
            {
                gaugeDict.Remove(element);
                if ((int) aura < 5)
                {
                    aura = Aura.NONE;
                }

                aura = aura switch
                {
                    Aura.ELECTROCHARGED => element == Element.HYDRO ? Aura.ELECTRO : Aura.HYDRO,
                    Aura.FROZEN => element == Element.HYDRO ? Aura.CRYO : Aura.HYDRO,
                    _ => aura
                };
                
            }
            gauge.GaugeValue -= value;
        }
        

        private void RemoveFrozen()
        {
            FreezeDuration = (0);
            if (gaugeDict.ContainsKey(Element.CRYO))
            {
                aura = Aura.CRYO;
            } else if (gaugeDict.ContainsKey(Element.HYDRO))
            {
                aura = Aura.HYDRO;
            }
            else
            {
                aura = Aura.NONE;
            }
        }

        private void DecreaseFrozen(double strength)
        {
            FreezeAura = (Math.Min(FreezeAura - strength, 0));
            if (FreezeAura == 0)
            {
                RemoveFrozen();
            }
        }

        private void SetFrozen(double auraStrength, double triggerStrength)
        {
            aura = Aura.FROZEN;
            triggerStrength *= 0.8;
            FreezeAura = (2 * Math.Min(auraStrength, triggerStrength));
            // KQM lists the formula as 2*sqrt(5*FreezeStrength+4)-4, however they are also stupid and use aura tax
            // instead of multiplying reactions strengths by 5/4 which makes this formula and other things simpler
            FreezeDuration = (4 * (Math.Sqrt(FreezeAura) - 1));
        }
    }
}