using System.Linq;
using System.Collections.Generic;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Unit
    {
        protected readonly int constellationLevel;
        protected readonly UnitStats baseStats;
        protected readonly Dictionary<string, UnitStats> buffs;

        protected Unit(int constellationLevel, UnitStats baseStats)
        {
            this.constellationLevel = constellationLevel;
            this.baseStats = baseStats;
            this.buffs = new Dictionary<string, UnitStats>();
        }

        protected virtual UnitStats GetCurrentStats()
        {
            UnitStats currentStats = baseStats;

            foreach(var buff in buffs.Values) currentStats += buff;

            return currentStats;
        }

        public void AddBuff(string id, UnitStats buff)
        {
            buffs.Add(id, buff);
        }
    }
}