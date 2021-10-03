using Tcc.Elements;
using Tcc.Stats;
using Tcc.Weapons;

namespace Tcc.Units
{
    public abstract class Anemo: Unit
    {
        protected Anemo(int constellationLevel, WeaponType weaponType, int burstEnergyCost,
            CapacityStats capacityStats, GeneralStats generalStats, AbilityStats burst, AbilityStats skill, 
            AbilityStats normal, AbilityStats charged, AbilityStats plunge) : 
            base(constellationLevel, Element.ANEMO, weaponType, burstEnergyCost, 
                capacityStats, generalStats, burst, skill, normal, charged,plunge)
        {
            Infusion = Element.PHYSICAL;
        }
        
        public Element Infusion { get; set; }

        private static bool IsUltInfused(object[] param)
        {
            var unit = (Anemo) param[0];
            return unit.Infusion != Element.PHYSICAL;
        }
    }
}