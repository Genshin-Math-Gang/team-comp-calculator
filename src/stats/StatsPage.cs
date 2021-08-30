using Tcc.Elements;

namespace Tcc.Stats
{
    public class StatsPage
    {
        public readonly CapacityStats capacityStats;
        public readonly GeneralStats generalStats;

        public StatsPage(CapacityStats capacityStats, GeneralStats generalStats)
        {
            this.capacityStats = capacityStats;
            this.generalStats = generalStats;
        }

        public MultipliableStat MaxHp => capacityStats.Hp;
        public int MaxEnergy => capacityStats.Energy;

        public int Level => generalStats.Level;
        public MultipliableStat Attack => generalStats.Attack;
        public MultipliableStat Defence => generalStats.Defence;
        public double ElementalMastery => generalStats.ElementalMastery;
        public double CritRate => generalStats.CritRate;
        public double CritDamage => generalStats.CritDamage;
        public double HealingBonus => generalStats.HealingBonus;
        public double IncomingHealingBonus => generalStats.IncomingHealingBonus;
        public double EnergyRecharge => generalStats.EnergyRecharge;
        public double CdReduction => generalStats.CdReduction;
        public double ShieldStrength => generalStats.ShieldStrength;
        public double DamagePercent => generalStats.DamagePercent;
        public double IndependentMultiplier => generalStats.IndependentMultiplier;
        public KeyedStatBonus<Element> ElementalBonus => generalStats.ElementalBonus;
        public KeyedPercentBonus<Element> ElementalResistance => generalStats.ElementalResistance;

        public static StatsPage operator +(StatsPage page, GeneralStats generalStats)
        {
            return new StatsPage(
                capacityStats: page.capacityStats,
                generalStats: page.generalStats + generalStats
            );
        }
    }
}