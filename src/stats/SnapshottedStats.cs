using Tcc.Events;
using Tcc.Units;

namespace Tcc.Stats
{
    public class SnapshottedStats
    {
        readonly Unit unit;
        readonly Types type;

        Stats stats;

        public SnapshottedStats(Unit unit, Types type)
        {
            this.unit = unit;
            this.type = type;
        }

        public Stats GetStats(Enemy.Enemy enemy, Timestamp timestamp)
        {
            return unit.AddStatsFromEnemy(stats, type, enemy, timestamp);
        }

        public WorldEvent Snapshot(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (_) => stats = unit.GetStatsFromUnit(type, timestamp));
        }
    }
}