using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs
{
    public delegate CapacityStats CapacityModifier();
    public delegate GeneralStats FirstPassModifier((Unit unit, Timestamp timestamp, CapacityStats capacityStats) data);
    public delegate GeneralStats SecondPassModifier((Unit unit, Timestamp timestamp, StatsPage firstPassStats) data);
    public delegate GeneralStats EnemyBasedModifier((Unit unit, Timestamp timestamp, Enemy.Enemy enemy) data);
    public delegate AbilityStats AbilityModifier((Unit unit, Timestamp timestamp) data);
}