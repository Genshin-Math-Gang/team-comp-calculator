namespace Tcc.Stats
{
    public class CombinedStats: UnitStats
    {
        readonly UnitStats stats1, stats2;

        public CombinedStats(UnitStats stats1, UnitStats stats2)
        {
            this.stats1 = stats1;
            this.stats2 = stats2;
        }

        public override double Attack => stats1.Attack + stats2.Attack;
    }
}
