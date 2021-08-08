using System.Collections.Generic;
using Tcc.Events;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Ayaka: Unit
    {
        UnitStats burstStats;

        public Ayaka(int constellationLevel): base(constellationLevel, new StaticUnitStats(100))
        {
        }

        public List<WorldEvent> Burst(double timestamp)
        {
            var list = new List<WorldEvent> {
                new WorldEvent(timestamp, (_) => burstStats = GetCurrentStats().snapshot()),
            };

            for(int hit = 0; hit < 20; hit++) list.Add(new Hit(timestamp + hit * 0.3, () => burstStats));

            return list;
        }
    }
}