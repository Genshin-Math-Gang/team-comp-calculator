using System.Collections.Generic;
using Tcc.Events;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Xiangling: Unit
    {
        UnitStats burstStats;

        public Xiangling(int constellationLevel): base(constellationLevel, new StaticUnitStats(100))
        {
        }

        public List<WorldEvent> InitialBurst(double timestamp) => new List<WorldEvent> {
            new Hit(timestamp, GetCurrentStats),
            new Hit(timestamp + 0.5, GetCurrentStats),
            new Hit(timestamp + 1.0, GetCurrentStats),
            new WorldEvent(timestamp + 1.0, (_) => burstStats = GetCurrentStats().snapshot()),
        };

        public List<WorldEvent> BurstHit(double timestamp) => new List<WorldEvent> {
            new Hit(timestamp, () => burstStats),
        };
    }
}