namespace Tcc.Stats
{
    public class CapacityStats
    {
        public MultipliableStat Hp { get; }
        public int Energy { get; }

        public CapacityStats(double baseHp = 0, double flatHp = 0, double hpPercent = 0, int energy = 0)
            : this(hp: new MultipliableStat(baseValue: baseHp, flatBonus: flatHp, percentBonus: hpPercent), energy: energy)
        {
        }

        public CapacityStats(MultipliableStat hp, int energy)
        {
            this.Hp = hp;
            this.Energy = energy;
        }

        public static CapacityStats operator +(CapacityStats first, CapacityStats second)
        {
            return new CapacityStats(
                hp: first.Hp + second.Hp,
                energy: first.Energy + second.Energy
            );
        }
        
        public static CapacityStats operator -(CapacityStats first, CapacityStats second)
        {
            return new CapacityStats(
                hp: first.Hp - second.Hp,
                energy: first.Energy - second.Energy
            );
        }
    }
}