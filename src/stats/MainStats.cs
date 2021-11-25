using System.Collections;

namespace Tcc.stats
{
    public class MainStats: IEnumerable
    {
        public Stats[] StatsArray;
        public Stats Sands() => StatsArray[0];
        public Stats Goblet() => StatsArray[1];
        public Stats Circlet() => StatsArray[2];
        public Stats this[int i] => StatsArray[i];

        public MainStats(Stats sand, Stats goblet, Stats circlet)
        {
            StatsArray = new[] {sand, goblet, circlet};
        }

        public IEnumerator GetEnumerator()
        {
            return StatsArray.GetEnumerator();
        }
    }
}