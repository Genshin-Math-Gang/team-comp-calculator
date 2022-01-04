using Tcc.enemy;
using Tcc.stats;
using Tcc.units;

namespace Tcc.buffs
{
    public delegate CapacityStats CapacityModifier();
    public delegate StatsPage FirstPassModifier((StatObject st, double timestamp) data);
    public delegate StatsPage SecondPassModifier((StatObject st, double timestamp, StatsPage firstPassStats) data);
    public delegate StatsPage EnemyBasedModifier((StatObject st, double timestamp, Enemy enemy, StatsPage firstPassStats) data);
    public delegate StatsPage ElementBasedModifier((StatObject st, double timestamp, StatsPage firstPassStats) data);
    public delegate AbilityStats AbilityModifier((StatObject st, double timestamp, StatsPage firstPassStats) data);
}