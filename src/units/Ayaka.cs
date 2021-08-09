using System.Collections.Generic;
using Tcc.Events;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Ayaka: Unit
    {
        public Ayaka(int constellationLevel): base(constellationLevel, new StaticUnitStats(100))
        {
        }

        public List<WorldEvent> Burst(double timestamp)
        {
            var list = new List<WorldEvent>();

            for(int hit = 0; hit < 20; hit++) list.Add(new Hit(timestamp + hit * 0.3, GetCurrentStats));

            return list;
        }
    }
}