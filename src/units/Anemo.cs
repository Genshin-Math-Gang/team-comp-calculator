using Tcc.Elements;
using Tcc.Stats;
using Tcc.Weapons;

namespace Tcc.Units
{
    public abstract class Anemo: Unit
    {

        public Element Infusion = Element.PHYSICAL;
        protected Anemo(int constellationLevel, WeaponType weaponType, int burstEnergyCost,
            CapacityStats capacityStats, GeneralStats generalStats, AbilityStats burst, AbilityStats skill, 
            AbilityStats normal, AbilityStats charged, AbilityStats plunge) : 
            base(constellationLevel, Element.ANEMO, weaponType, burstEnergyCost, 
                capacityStats, generalStats, burst, skill, normal, charged,plunge)
        {
        }
        
        

        public void SetInfusion(Element e)
        {
            Infusion = e;
        }

        public Element GetInfusion()
        {
            return Infusion;
        }
    }
}