using Tcc.Events;
using Tcc.Units;
using System;

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

        public Stats GetStats(Timestamp timestamp)
        {
            unit.buffsFromUnit.RemoveAll((buff) => buff.HasExpired(timestamp));
            unit.buffsFromStats.RemoveAll((buff) => buff.HasExpired(timestamp));

            var firstPassStats = unit.modifiers[type] + stats;
            foreach(var buff in unit.buffsFromUnit) 
            {
                if (buff.type == type) 
                firstPassStats += buff.GetModifier(unit, type);
            }

            var result = firstPassStats;
            foreach(var buff in unit.buffsFromStats) 
            {
                if (buff.type == type) 
                result += buff.GetModifier(unit, firstPassStats, timestamp, type);
            }

            //result = unit.AddStatsFromEnemy(result, type, timestamp);
            return result;
        }

        public WorldEvent Snapshot(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (_) => {stats = unit.SnapshotStats(timestamp); System.Console.WriteLine($"Snapshotted {this.unit} {this.type}");});
        }
    }
}