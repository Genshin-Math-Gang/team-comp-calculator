using System;
using Tcc.stats;

namespace Tcc.elements
{
    [Flags]
    public enum Element
    {
        PYRO = 0b1,
        HYDRO = 0b10,
        ELECTRO = 0b100,
        CRYO = 0b1000,
        ANEMO = 0b10000,
        GEO = 0b100000,
        DENDRO = 0b1000000,
        PHYSICAL = 0b10000000,
        ANY = PHYSICAL | PYRO | HYDRO | CRYO | GEO | DENDRO
    }

    public static class Converter
    {
        public static Element AuraToElement(Aura aura)
        {
            return aura switch
            {
                Aura.PYRO => Element.PYRO,
                Aura.HYDRO => Element.HYDRO,
                Aura.ELECTRO => Element.ELECTRO,
                Aura.CRYO => Element.CRYO,
                Aura.ELECTROCHARGED => Element.HYDRO,
                Aura.FROZEN => Element.CRYO, // maybe this needs to be changed
                Aura.NONE => Element.PHYSICAL,
                _ => throw new ArgumentOutOfRangeException(nameof(aura), aura, null)
            }; 
        }
        
        public static Aura ElementToAura (Element element)
        {
            return element switch
            {
                Element.PYRO => Aura.PYRO,
                Element.CRYO => Aura.CRYO,
                Element.HYDRO => Aura.HYDRO,
                Element.ELECTRO => Aura.ELECTRO,
                _ => Aura.NONE
            };
        }

        public static Stats ElementToRes(Element element)
        {
            return element switch
            {
                Element.PYRO => Stats.PyroResistance,
                Element.CRYO => Stats.CryoResistance,
                Element.HYDRO => Stats.HydroResistance,
                Element.ELECTRO => Stats.ElectroResistance,
                Element.ANEMO => Stats.AnemoResistance,
                Element.GEO => Stats.GeoResistance,
                Element.PHYSICAL => Stats.PhysicalResistance,
                Element.DENDRO => Stats.DendroResistance,
                _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
            };
        }
        
        public static Stats ElementToBonus(Element element)
        {
            return element switch
            {
                Element.PYRO => Stats.PyroDamageBonus,
                Element.CRYO => Stats.CryoDamageBonus,
                Element.HYDRO => Stats.HydroDamageBonus,
                Element.ELECTRO => Stats.ElectroDamageBonus,
                Element.ANEMO => Stats.AnemoDamageBonus,
                Element.GEO => Stats.GeoDamageBonus,
                Element.PHYSICAL => Stats.PhysicalDamageBonus,
                Element.DENDRO => Stats.DendroDamageBonus,
                _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
            };
        }
    }
}