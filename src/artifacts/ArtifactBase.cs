using System.Collections.Generic;
using Tcc.Stats;

namespace Tcc.Buffs.Artifacts
{
    public abstract class ArtifactBase<T> where T: ArtifactSet
    {
        private static Dictionary<Stats.Stats, double> AverageSubstatRoll = new Dictionary<Stats.Stats, double>
        {
            {Stats.Stats.HpFlat, 253.94},
            {Stats.Stats.AtkFlat, 16.535},
            {Stats.Stats.DefFlat, 19.675},
            {Stats.Stats.HpPercent, 0.04955},
            {Stats.Stats.AtkPercent, 0.04955},
            {Stats.Stats.DefPercent, 0.06195},
            {Stats.Stats.ElementalMastery, 19.815},
            {Stats.Stats.EnergyRecharge, 0.05505},
            {Stats.Stats.CritRate, 0.03305},
            {Stats.Stats.CritDamage, 0.0661}
        };
        
        private static Dictionary<Stats.Stats, double> MainStatValues = new Dictionary<Stats.Stats, double>
        {
            {Stats.Stats.HpFlat, 4780},
            {Stats.Stats.AtkFlat, 311},
            {Stats.Stats.HpPercent, 0.466},
            {Stats.Stats.AtkPercent, 0.466},
            {Stats.Stats.DefPercent, 0.583},
            {Stats.Stats.ElementalMastery, 186.5},
            {Stats.Stats.EnergyRecharge, 0.518},
            {Stats.Stats.CritRate, 0.311},
            {Stats.Stats.CritDamage, 0.622},
            {Stats.Stats.PyroDamageBonus, 0.466},
            {Stats.Stats.HydroDamageBonus, 0.466},
            {Stats.Stats.ElectroDamageBonus, 0.466},
            {Stats.Stats.CryoDamageBonus, 0.466},
            {Stats.Stats.AnemoDamageBonus, 0.466},
            {Stats.Stats.GeoDamageBonus, 0.466},
            {Stats.Stats.DendroDamageBonus, 0.466},
            {Stats.Stats.PhysicalDamageBonus, 0.583},
            {Stats.Stats.HealingBonus, 0.359}
        };
        
        
        public Stats.Stats MainStat;
        public ArtifactSlots Slot;
        public Dictionary<Stats.Stats, int> SubstatRolls;
        public StatsPage ArtifactStats;

        public ArtifactBase(ArtifactSlots artifactSlot, Stats.Stats mainStat)
        {
            // TODO: add validation
            Slot = artifactSlot;
            MainStat = mainStat;
            ArtifactStats = new StatsPage();
            
        }
        
       // public void AddSubstat()
    }
}