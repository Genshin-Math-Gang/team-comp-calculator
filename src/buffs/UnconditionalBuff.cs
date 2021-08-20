using System.Collections.Generic;
using System;
using Tcc.Units;

namespace Tcc.Buffs
{
    public abstract class UnconditionalBuff: Expirable
    {
        public UnconditionalBuff(Guid id, Timestamp expiryTime): base(id, expiryTime)
        {
        }

        public abstract void AddToUnit(Unit unit, List<UnconditionalBuff> buffs);

        public abstract Stats.Stats GetModifier(Stats.Types type);
    }
}