namespace Tcc.Stats
{
    public class DamagePercentAndStats
    {
        readonly GeneralStats generalStats;
        readonly double damagePercentBonus;

        public DamagePercentAndStats(GeneralStats generalStats = null, double damagePercentBonus = 0)
        {
            this.generalStats = generalStats;
            this.damagePercentBonus = damagePercentBonus;
        }
    }
}