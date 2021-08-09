using System.Collections.Generic;
using Tcc.Events;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Xiangling: Unit
    {
        UnitStats skillStats;
        UnitStats burstStats;

        public Xiangling(int constellationLevel): base(constellationLevel, new StaticUnitStats(100))
        {
        }

        public List<WorldEvent> Skill(double timestamp) => new List<WorldEvent> {
            new WorldEvent(timestamp, (_) => skillStats = GetCurrentStats().snapshot()),
            new Hit(timestamp, () => skillStats),
            new Hit(timestamp + 2.5, () => skillStats),
            new Hit(timestamp + 5.0, () => skillStats),
            new Hit(timestamp + 7.5, () => skillStats),
        };

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