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
        
        // maybe make an in place add to try to speedup some stuff
        public static SecondPassStatsPage operator +(SecondPassStatsPage page, StatsPage statsPage)
        {
            return new SecondPassStatsPage(
                firstPassStats: page.firstPassStats + statsPage

            );
        }

    }
}