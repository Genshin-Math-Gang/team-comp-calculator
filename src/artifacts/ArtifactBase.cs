using System.Collections.Generic;
using Tcc.stats;

namespace Tcc.artifacts
{
    public abstract class ArtifactBase<T> where T: ArtifactSet
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
        
        
        public Stats MainStat { get; }
        public ArtifactSlots Slot { get; }
        public Dictionary<Stats, int> SubstatRolls;
        public StatsPage ArtifactStats { get; private set; }

        public ArtifactBase(ArtifactSlots artifactSlot, Stats mainStat)
        {
            // TODO: add validation
            Slot = artifactSlot;
            MainStat = mainStat;
            SubstatRolls = new Dictionary<Stats, int>();
            ArtifactStats = new StatsPage(mainStat, MainStatValues[mainStat]);
            
        }

        public void AddSubstat(Stats stats)
        {
            if (SubstatRolls.ContainsKey(stats))
            {
                SubstatRolls[stats] += 1;
            }
            else
            {
                SubstatRolls[stats] = 1;
            }

            ArtifactStats += (stats, SubstatRolls[stats]);
        }
        
        public bool RemoveSubstat(Stats stats)
        {
            if (!SubstatRolls.ContainsKey(stats) || SubstatRolls[stats]==0) return false;
            SubstatRolls[stats] -= 1;
            ArtifactStats -= (stats, SubstatRolls[stats]);
            return true;

        }
    }
}