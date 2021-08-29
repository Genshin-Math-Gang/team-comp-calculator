namespace Tcc.Stats
{
    public class AbilityStats
    {
        readonly GeneralStats generalStats;
        readonly double[] motionValues;

        public AbilityStats(GeneralStats generalStats = null, double[] motionValues = null)
        {
            this.generalStats = generalStats;
            this.motionValues = motionValues;
        }

        public static implicit operator AbilityStats(GeneralStats generalStats) => new AbilityStats(generalStats: generalStats);
    }
}