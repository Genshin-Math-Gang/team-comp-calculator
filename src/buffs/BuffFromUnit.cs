using System.Collections.Generic;
using System;
using Tcc.Units;

namespace Tcc.Buffs
{
    public abstract class BuffFromUnit: Expirable
    {
        public BuffFromUnit(Guid id, Timestamp expiryTime): base(id, expiryTime)
        {
        }

        public abstract void AddToUnit(Unit unit, List<BuffFromUnit> buffs);

        public abstract Stats.Stats GetModifier(Unit unit, Stats.Types type);
    }
}