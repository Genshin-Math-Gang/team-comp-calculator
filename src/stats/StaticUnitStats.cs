namespace Tcc.Stats
{
    public class StaticUnitStats: UnitStats
    {
        public StaticUnitStats(double attack)
        {
            this.Attack = attack;
        }

        public override double Attack { get; }
    }
}
