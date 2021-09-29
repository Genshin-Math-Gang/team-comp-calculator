using Tcc.Events;
using Tcc.Units;

namespace Tcc.Stats
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

        public SecondPassStatsPage GetStats(Timestamp _) => stats;

        public WorldEvent Snapshot(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (_) =>
            {
                stats = unit.GetStatsPage(timestamp);
                System.Console.WriteLine($"Snapshotted {unit} {type}");
            }, priority:1);
        }
    }
}