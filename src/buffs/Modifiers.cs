using Tcc.enemy;
using Tcc.stats;
using Tcc.units;

namespace Tcc.buffs
{
    public delegate CapacityStats CapacityModifier();
    public delegate StatsPage FirstPassModifier((StatObject st, Timestamp timestamp) data);
    public delegate StatsPage SecondPassModifier((StatObject st, Timestamp timestamp, StatsPage firstPassStats) data);
    public delegate StatsPage EnemyBasedModifier((StatObject st, Timestamp timestamp, Enemy enemy, StatsPage firstPassStats) data);
    public delegate StatsPage ElementBasedModifier((StatObject st, Timestamp timestamp, StatsPage firstPassStats) data);
    public delegate AbilityStats AbilityModifier((StatObject st, Timestamp timestamp, StatsPage firstPassStats) data);
}