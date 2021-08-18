using System;
using System.Collections.Generic;

namespace Tcc.Elements
{
    
    // electro and hydro can coexist, frozen is weird 
    // for now i'm just going to assume valid constructions are given
    public class Gauge
    {
        
        private Dictionary<Element, GaugeElement> gaugeDict;
        private double LastChecked { get; set; }
        private Aura aura = Aura.NONE;
        private double FreezeDuration = 0;
        private double FreezeAura = 0;

        public Gauge(double timestamp)
        {
            this.gaugeDict = new Dictionary<Element, GaugeElement> { };
            this.LastChecked = timestamp;

        }
        
        
        // change return type later
        public void ElementApplied(double timestamp, Element elementType, double gaugeStrength, bool isHeavy=false)
        {
            TimeDecay(timestamp);
            if (isHeavy && aura == Aura.FROZEN)
            {
                RemoveFrozen();
            }
            if (aura == ElementToAura(elementType))
            {
                gaugeDict[elementType].UpdateGauge(gaugeStrength);
                return;
            } 
            if (elementType == Element.PHYSICAL)
            {
                return;
            }
            // garbage code ensues
            var strength = gaugeStrength * 1.25;
            // need to do something else regarding damage but for now i just want to track aura properly
            // swirl is terrifying 
            // frozen is weird
            switch (aura)
            {
                case Aura.NONE:
                    gaugeDict[elementType] = new GaugeElement(elementType, gaugeStrength);
                    aura = ElementToAura(elementType);
                    break;
                case Aura.PYRO:
                    switch (elementType)
                    {
                        case Element.HYDRO:
                            strength *= 2;
                            break;
                        case Element.CRYO:
                            strength /= 2;
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
                    DecreaseElement(Element.PYRO, strength);
                    break;
                case Aura.HYDRO:
                    switch (elementType)
                    {
                        case Element.PYRO:
                            strength /= 2;
                            break;
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
                            break;
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
                            return;
                        case Element.HYDRO:
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
                            strength = 0;
                            break;
                        case Element.CRYO:
                            break;
                        case Element.ELECTRO:
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
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
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
                            strength = 0;
                            break;
                        case Element.CRYO:
                            gaugeDict[elementType].UpdateGauge(gaugeStrength);
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
            
            
        }

        private void TimeDecay(double timestamp)
        {
            if (timestamp < LastChecked)
            {
                throw new ArgumentException("time input before last checked time");
            }

            double timeSince = timestamp - LastChecked;
            LastChecked = timestamp;
            // this is pretty scuffed but it should work
            // also i'm just ignoring how hit lag can change EC slightly because that is a mess
            if (aura == Aura.ELECTROCHARGED)
            {
                int timer = 0;
                while (timer < timeSince)
                {
                    timer += 1;
                    gaugeDict[Element.ELECTRO].TimeDecay(1);
                    gaugeDict[Element.HYDRO].TimeDecay(1);
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
                FreezeDuration = Math.Min(0, FreezeDuration - timeSince);
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

        private Aura ElementToAura(Element element)
        {
            if ((int) element < 5)
            {
                return (Aura) element;
            }

            return Aura.NONE;
        }

        private void RemoveFrozen()
        {
            FreezeDuration = 0;
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
            FreezeAura = Math.Min(FreezeAura - strength, 0);
            if (FreezeAura == 0)
            {
                RemoveFrozen();
            }
        }

        private void SetFrozen(double auraStrength, double triggerStrength)
        {
            aura = Aura.FROZEN;
            triggerStrength *= 0.8;
            FreezeAura = 2 * Math.Min(auraStrength, triggerStrength);
            // KQM lists the formula as 2*sqrt(5*FreezeStrength+4)-4, however they are also stupid and use aura tax
            // instead of multiplying reactions strengths by 5/4 which makes this formula and other things simpler
            FreezeDuration = 4 * (Math.Sqrt(FreezeAura) - 1);
        }
    }
}