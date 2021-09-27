using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs
{
    public delegate CapacityStats CapacityModifier();
    public delegate GeneralStats FirstPassModifier((StatObject st, Timestamp timestamp, CapacityStats capacityStats) data);
    public delegate GeneralStats SecondPassModifier((StatObject st, Timestamp timestamp, StatsPage firstPassStats) data);
    public delegate GeneralStats EnemyBasedModifier((StatObject st, Timestamp timestamp, Enemy.Enemy enemy, StatsPage firstPassStats) data);
    public delegate GeneralStats ElementBasedModifier((StatObject st, Timestamp timestamp, StatsPage firstPassStats) data);
    public delegate AbilityStats AbilityModifier((StatObject st, Timestamp timestamp, StatsPage firstPassStats) data);
}