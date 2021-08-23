using System.Collections.Generic;
using System;
using Tcc.Units;

namespace Tcc.Buffs
{
    public abstract class BuffFromStats: Expirable
    {
        public BuffFromStats(Guid id, Timestamp expiryTime): base(id, expiryTime)
        {
        }

        public abstract void AddToUnit(Unit unit, List<BuffFromStats> buffs);

        public abstract Stats.Stats GetModifier(Unit unit, Stats.Stats firstPassStats, Stats.Types type);
    }
}