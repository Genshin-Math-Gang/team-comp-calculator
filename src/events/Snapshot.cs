using System;
using Tcc.Stats;

namespace Tcc.Events
{
    public class Snapshot: WorldEvent
    {
        public Snapshot(Timestamp timestamp, Func<Timestamp, Stats.Stats> stats, Action<Stats.Stats> snapshot, string description = ""): base(
            timestamp,
            (world) => snapshot(stats(timestamp))
        ) {
        }
    }
}