using Tcc.events;
using Tcc.units;

namespace Tcc.stats
{
    public class SnapshottedStats
    {
        readonly Unit unit;
        readonly Types type;

        SecondPassStatsPage stats;

        public SnapshottedStats(Unit unit, Types type)
        {
            this.unit = unit;
            this.type = type;
        }

        public SecondPassStatsPage GetStats(double _) => stats;

        public WorldEvent Snapshot(double timestamp)
        {
            return new WorldEvent(timestamp, (_) =>
            {
                stats = unit.GetStatsPage(timestamp);
                System.Console.WriteLine($"Snapshotted {unit} {type}");
            }, priority:1);
        }
    }
}