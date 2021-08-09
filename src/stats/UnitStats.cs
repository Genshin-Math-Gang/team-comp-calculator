namespace Tcc.Stats
{
    public class UnitStats
    {
        // Simplified for prototype, real impl would have all stats
        public virtual double Attack { get; }

        public double CalculateHitDamage() => Attack;

        public StaticUnitStats snapshot() => new StaticUnitStats(Attack);

        public static UnitStats operator +(UnitStats a, UnitStats b) => new CombinedStats(a, b);
    }
}
