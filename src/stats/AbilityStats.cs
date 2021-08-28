namespace Tcc.Stats
{
    public class AbilityStats
    {
        readonly GeneralStats generalStats;
        readonly double damagePercentBonus;
        readonly double[] motionValues;

        public AbilityStats(GeneralStats generalStats = null, double damagePercentBonus = 0, double[] motionValues = null)
        {
            this.generalStats = generalStats;
            this.damagePercentBonus = damagePercentBonus;
            this.motionValues = motionValues;
        }
    }
}