namespace Tcc.stats
{
    public class SecondPassStatsPage: StatsPage
    {
        public readonly StatsPage firstPassStats;

        public SecondPassStatsPage(StatsPage firstPassStats)
        {
            this.firstPassStats = firstPassStats;
        }



        /*public static SecondPassStatsPage operator +(SecondPassStatsPage page, GeneralStats generalStats)
        {
            return new SecondPassStatsPage(
                firstPassStats: page.firstPassStats,
                generalStats: page.GeneralStats + generalStats
            );
        }*/
        
        public static SecondPassStatsPage operator +(SecondPassStatsPage page, StatsPage statsPage)
        {
            return new SecondPassStatsPage(
                firstPassStats: page.firstPassStats + statsPage

            );
        }

    }
}