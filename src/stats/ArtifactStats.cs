using System.Collections.Generic;

namespace Tcc.stats
{
    public class ArtifactStats: StatsPage
    {
        private static Dictionary<Stats, double> AverageSubstatRoll = new Dictionary<Stats, double>
        {
            {Stats.HpFlat, 253.94},
            {Stats.AtkFlat, 16.535},
            {Stats.DefFlat, 19.675},
            {Stats.HpPercent, 0.04955},
            {Stats.AtkPercent, 0.04955},
            {Stats.DefPercent, 0.06195},
            {Stats.ElementalMastery, 19.815},
            {Stats.EnergyRecharge, 0.05505},
            {Stats.CritRate, 0.03305},
            {Stats.CritDamage, 0.0661}
        };
        
        private static Dictionary<Stats, double> MainStatValues = new Dictionary<Stats, double>
        {
            {Stats.HpFlat, 4780},
            {Stats.AtkFlat, 311},
            {Stats.HpPercent, 0.466},
            {Stats.AtkPercent, 0.466},
            {Stats.DefPercent, 0.583},
            {Stats.ElementalMastery, 186.5},
            {Stats.EnergyRecharge, 0.518},
            {Stats.CritRate, 0.311},
            {Stats.CritDamage, 0.622},
            {Stats.PyroDamageBonus, 0.466},
            {Stats.HydroDamageBonus, 0.466},
            {Stats.ElectroDamageBonus, 0.466},
            {Stats.CryoDamageBonus, 0.466},
            {Stats.AnemoDamageBonus, 0.466},
            {Stats.GeoDamageBonus, 0.466},
            {Stats.DendroDamageBonus, 0.466},
            {Stats.PhysicalDamageBonus, 0.583},
            {Stats.HealingBonus, 0.359}
        };
        
        public Dictionary<Stats, int> SubstatCount;
        public MainStats MainStats;

        /*public ArtifactStats(): 
            base(new CapacityStats(flatHp: MainStatValues[Stats.HpFlat]),
                new GeneralStats(flatAttack: MainStatValues[Stats.AtkFlat]))
        {
            MainStats = new MainStats(Stats.AtkPercent, Stats.AtkPercent, Stats.AtkPercent);
        }

        public ArtifactStats(List<Stats> mainStats) :
            base(new CapacityStats(flatHp: MainStatValues[Stats.HpFlat]),
                new GeneralStats(flatAttack: MainStatValues[Stats.AtkFlat]))
        {
            
        }

        private void setMainStats(List<Stats> mainsStats)
        {
            foreach (Stats slot in MainStats)
            {
                if (slot == Stats.HpPercent)
                {
                    CapacityStats += new CapacityStats(hpPercent: -MainStatValues[Stats.HpPercent]);
                }
            }
        }*/
    }
    
    
}