using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs
{
    public delegate CapacityStats CapacityModifier();
    public delegate StatsPage FirstPassModifier((StatObject st, Timestamp timestamp) data);
    public delegate StatsPage SecondPassModifier((StatObject st, Timestamp timestamp, StatsPage firstPassStats) data);
    public delegate StatsPage EnemyBasedModifier((StatObject st, Timestamp timestamp, Enemy.Enemy enemy, StatsPage firstPassStats) data);
    public delegate StatsPage ElementBasedModifier((StatObject st, Timestamp timestamp, StatsPage firstPassStats) data);
    public delegate AbilityStats AbilityModifier((StatObject st, Timestamp timestamp, StatsPage firstPassStats) data);
}