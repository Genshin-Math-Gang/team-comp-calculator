using System;
using System.Collections.Generic;
using Tcc.Stats;
using Tcc.Events;

namespace Tcc.Elements
{
    
    // electro and hydro can coexist, frozen is weird 
    // for now i'm just going to assume valid constructions are given
    public class Gauge
    {        
        private Dictionary<Element, GaugeElement> gaugeDict = new Dictionary<Element, GaugeElement>();
        private Timestamp LastChecked = new Timestamp(0);
        private Dictionary<Tuple<Units.Unit, Types>, Timestamp> ICDtimer = new Dictionary<Tuple<Units.Unit, Types>, Timestamp>();
        private Dictionary<Tuple<Units.Unit, Types>, int> hitPity = new Dictionary<Tuple<Units.Unit, Types>, int>();
        private Aura aura = Aura.NONE;
        private Timestamp FreezeDuration = new Timestamp(0);
        private Timestamp FreezeAura = new Timestamp(0);

        public Gauge() {}
        
        public double ElementApplied(Timestamp timestamp, Element elementType, Units.Unit unit, Types type, bool isHeavy=false)
        {
            if (type == Types.TRANSFORMATIVE)
            {
                return Reaction.NONE;
            }
            Tuple<Units.Unit, Types> key = new Tuple<Units.Unit, Types>(unit, type);
            if (!ICDtimer.ContainsKey(key))
            {
                ICDtimer.Add(key, timestamp);
                hitPity.Add(key, 1);
            }
            else
            {
                hitPity[key] += 1;
                
                if ((timestamp - ICDtimer[key] <= unit.stats.ICD) && (hitPity[key] != unit.stats.HitPity)) return Reaction.NONE;

                if (hitPity[key] == unit.stats.HitPity) hitPity[key] = 0;

                if (timestamp - ICDtimer[key] > unit.stats.ICD) {hitPity[key] = 0; ICDtimer[key] = timestamp;}
            }

            if (aura == Aura.NONE) 
            {
                gaugeDict.Add(elementType, new GaugeElement(elementType, unit.modifiers[type].GaugeStrength)); 
                LastChecked = timestamp; 
                this.aura = ElementToAura(elementType);
                return Reaction.NONE;
            } 
            else TimeDecay(timestamp);

            if (isHeavy && aura == Aura.FROZEN)
            {
                RemoveFrozen();
            }
            if (aura == ElementToAura(elementType))
            {
                gaugeDict[elementType].UpdateGauge(unit.modifiers[type].GaugeStrength);
                return Reaction.NONE;
            } 
            if (elementType == Element.PHYSICAL)
            {
                return Reaction.NONE;
            }

            var strength = unit.modifiers[type].GaugeStrength * 1.25;
            // need to do something else regarding damage but for now i just want to track aura properly
            // swirl is terrifying 
            // frozen is weird

            switch (aura)
            {
                case Aura.NONE:
                    gaugeDict[elementType] = new GaugeElement(elementType, unit.modifiers[type].GaugeStrength);
                    aura = ElementToAura(elementType);
                    return Reaction.NONE;
                case Aura.PYRO:
                    switch (elementType)
                    {
                        case Element.HYDRO:
                            strength *= 2;
                            return 2 * (1 + 2.78 * unit.stats.ElementalMastery / (unit.stats.ElementalMastery + 1400) + unit.stats.ReactionBonus.GetDamagePercentBonus(Reaction.VAPORIZE));
                        case Element.CRYO:
                            strength /= 2;
                            return 1.5 * (1 + 2.78 * unit.stats.ElementalMastery / (unit.stats.ElementalMastery + 1400) + unit.stats.ReactionBonus.GetDamagePercentBonus(Reaction.MELT));
                        case Element.ELECTRO:
                            return Reaction.OVERLOADED;
                        case Element.ANEMO:
                            strength /= 2;
                            break;
                        case Element.GEO:
                            strength /= 2;
                            break;
                    }
                    DecreaseElement(Element.PYRO, strength);
                    break;
                case Aura.HYDRO:
                    switch (elementType)
                    {
                        case Element.PYRO:
                            strength /= 2;
                            return 1.5 * (1 + 2.78 * unit.stats.ElementalMastery / (unit.stats.ElementalMastery + 1400) + unit.stats.ReactionBonus.GetDamagePercentBonus(Reaction.VAPORIZE));
                        case Element.CRYO:
                            SetFrozen(gaugeDict[Element.HYDRO].GaugeValue, strength);
                            break;
                        case Element.ELECTRO:
                            aura = Aura.ELECTROCHARGED;
                            gaugeDict[elementType] = new GaugeElement(elementType, strength);
                            strength = 0;
                            break;
                        case Element.ANEMO:
                            strength /= 2;
                            break;
                        case Element.GEO:
                            strength /= 2;
                            break;
                    }
                    DecreaseElement(Element.HYDRO, strength);
                    break;
                case Aura.CRYO:
                    switch (elementType)
                    {
                        case Element.PYRO:
                            strength *= 2;
                            return 2 * (1 + 2.78 * unit.stats.ElementalMastery / (unit.stats.ElementalMastery + 1400) + unit.stats.ReactionBonus.GetDamagePercentBonus(Reaction.MELT));
                        case Element.HYDRO:
                            SetFrozen(gaugeDict[Element.CRYO].GaugeValue, strength);
                            break;
                        case Element.ELECTRO:
                            break;
                        case Element.ANEMO:
                            strength /= 2;
                            break;
                        case Element.GEO:
                            strength /= 2;
                            break;
                    }
                    DecreaseElement(Element.CRYO, strength);
                    break;
                case Aura.ELECTRO:
                    switch (elementType)
                    {
                        case Element.PYRO:
                            break;
                        case Element.HYDRO:
                            aura = Aura.ELECTROCHARGED;
                            gaugeDict[elementType] = new GaugeElement(elementType, strength);
                            strength = 0;
                            break;
                        case Element.CRYO:
                            break;
                        case Element.ANEMO:
                            strength /= 2;
                            break;
                        case Element.GEO:
                            strength /= 2;
                            break;
                    }
                    DecreaseElement(Element.ELECTRO, strength);
                    break;
                case Aura.ELECTROCHARGED:
                    switch (elementType)
                    {
                        case Element.PYRO:
                            DecreaseElement(Element.HYDRO, 2 * strength);
                            DecreaseElement(Element.ELECTRO, strength);
                            return 1.5 * (1 + 2.78 * unit.stats.ElementalMastery / (unit.stats.ElementalMastery + 1400) + unit.stats.ReactionBonus.GetDamagePercentBonus(Reaction.VAPORIZE));
                        case Element.HYDRO:
                            gaugeDict[elementType].UpdateGauge(unit.modifiers[type].GaugeStrength);
                            strength = 0;
                            break;
                        case Element.CRYO:
                            break;
                        case Element.ELECTRO:
                            gaugeDict[elementType].UpdateGauge(unit.modifiers[type].GaugeStrength);
                            strength = 0;
                            break;
                        case Element.ANEMO:
                            strength /= 2;
                            break;
                        case Element.GEO:
                            strength /= 2;
                            break;
                    }
                    DecreaseElement(Element.HYDRO, strength);
                    DecreaseElement(Element.ELECTRO, strength);
                    break;
                case Aura.FROZEN:
                    // frozen may be a bit inaccurate because it is very convoluted 
                    switch (elementType)
                    {
                        case Element.PYRO:
                            strength *= 2;
                            break;
                        case Element.HYDRO:
                            gaugeDict[elementType].UpdateGauge(unit.modifiers[type].GaugeStrength);
                            strength = 0;
                            break;
                        case Element.CRYO:
                            gaugeDict[elementType].UpdateGauge(unit.modifiers[type].GaugeStrength);
                            strength = 0;
                            break;
                        case Element.ELECTRO:
                            break;
                        case Element.ANEMO:
                            strength /= 2;
                            break;
                        case Element.GEO:
                            strength /= 2;
                            break;
                    }
                    DecreaseFrozen(strength);
                    if (gaugeDict.ContainsKey(Element.CRYO))
                    {
                        // if a frozen enemy has an underlying cryo aura that also has gauge consumed on reaction
                        DecreaseElement(Element.CRYO, strength);
                    }
                    // swirl and geo can cause double reaction if they are strong enough to fully consume the frozen
                    // this is kinda ugly but it should work
                    if (aura != Aura.FROZEN && gaugeDict.ContainsKey(Element.HYDRO) && 
                        elementType is Element.GEO or Element.ANEMO)
                    {
                        goto case Aura.HYDRO;
                    }
                    break;
            }
            System.Console.WriteLine("Something weird happened");
            return Reaction.NONE;
        }

        private void TimeDecay(Timestamp timestamp)
        {
            if (timestamp < LastChecked)
            {
                throw new ArgumentException("Time input before last checked, cannot go back in time");
            }

            Timestamp timeSince = timestamp - LastChecked;
            LastChecked = timestamp;

            // this is pretty scuffed but it should work
            // also i'm just ignoring how hit lag can change EC slightly because that is a mess
            if (aura == Aura.ELECTROCHARGED)
            {
                Timestamp timer = new Timestamp(0);
                while (timer < timeSince)
                {
                    timer += 1;
                    gaugeDict[Element.ELECTRO].TimeDecay(new Timestamp(1));
                    gaugeDict[Element.HYDRO].TimeDecay(new Timestamp(1));
                    DecreaseElement(Element.ELECTRO, 0.4);
                    DecreaseElement(Element.HYDRO, 0.4);
                    // trigger EC here
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
                if (gaugeDict[Element.ELECTRO].GaugeValue == 0 || gaugeDict[Element.ELECTRO].GaugeValue == 0)
                {
                    // trigger EC here
                }
            
            }
            if (aura == Aura.FROZEN)
            {
                FreezeDuration = new Timestamp(Math.Min(0, FreezeDuration - timeSince));
                if (FreezeDuration == 0)
                {
                    RemoveFrozen();
                }
            } 
            Time:
                foreach (var pair in gaugeDict)
                {
                    GaugeElement element = pair.Value;
                    element.TimeDecay(timeSince);
                }
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

        private Aura ElementToAura (Element element)
        {
            switch(element)
            {
                case Element.PYRO: return Aura.PYRO;
                case Element.CRYO: return Aura.CRYO;
                case Element.HYDRO: return Aura.HYDRO;
                case Element.ELECTRO: return Aura.ELECTRO;
            }
            return Aura.NONE;
        }

        private void RemoveFrozen()
        {
            FreezeDuration = new Timestamp(0);
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
            FreezeAura = new Timestamp(Math.Min(FreezeAura - strength, 0));
            if (FreezeAura == 0)
            {
                RemoveFrozen();
            }
        }

        private void SetFrozen(double auraStrength, double triggerStrength)
        {
            aura = Aura.FROZEN;
            triggerStrength *= 0.8;
            FreezeAura = new Timestamp(2 * Math.Min(auraStrength, triggerStrength));
            // KQM lists the formula as 2*sqrt(5*FreezeStrength+4)-4, however they are also stupid and use aura tax
            // instead of multiplying reactions strengths by 5/4 which makes this formula and other things simpler
            FreezeDuration = new Timestamp(4 * (Math.Sqrt(FreezeAura) - 1));
        }
    }
}