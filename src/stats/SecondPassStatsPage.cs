namespace Tcc.Stats
{
    public class SecondPassStatsPage: StatsPage
    {
        public readonly StatsPage firstPassStats;

        public SecondPassStatsPage(StatsPage firstPassStats)
            : base(firstPassStats.capacityStats)
        {
            this.firstPassStats = firstPassStats;
        }

        SecondPassStatsPage(StatsPage firstPassStats, GeneralStats generalStats)
            : base(firstPassStats.capacityStats, generalStats)
        {
            this.firstPassStats = firstPassStats;
        }

        public static SecondPassStatsPage operator +(SecondPassStatsPage page, GeneralStats generalStats)
        {
            return new SecondPassStatsPage(
                firstPassStats: page.firstPassStats,
                generalStats: page.generalStats + generalStats
            );
        }
    }
}