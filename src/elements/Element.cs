using System;

namespace Tcc.Elements
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

        public static Stats.Stats ElementToRes(Element element)
        {
            return element switch
            {
                Element.PYRO => Stats.Stats.PyroResistance,
                Element.CRYO => Stats.Stats.CryoResistance,
                Element.HYDRO => Stats.Stats.HydroResistance,
                Element.ELECTRO => Stats.Stats.ElectroResistance,
                Element.ANEMO => Stats.Stats.AnemoResistance,
                Element.GEO => Stats.Stats.GeoResistance,
                Element.PHYSICAL => Stats.Stats.PhysicalResistance,
                Element.DENDRO => Stats.Stats.DendroResistance,
                _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
            };
        }
        
        public static Stats.Stats ElementToBonus(Element element)
        {
            return element switch
            {
                Element.PYRO => Stats.Stats.PyroDamageBonus,
                Element.CRYO => Stats.Stats.CryoDamageBonus,
                Element.HYDRO => Stats.Stats.HydroDamageBonus,
                Element.ELECTRO => Stats.Stats.ElectroDamageBonus,
                Element.ANEMO => Stats.Stats.AnemoDamageBonus,
                Element.GEO => Stats.Stats.GeoDamageBonus,
                Element.PHYSICAL => Stats.Stats.PhysicalDamageBonus,
                Element.DENDRO => Stats.Stats.DendroDamageBonus,
                _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
            };
        }
    }
}