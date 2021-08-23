using System.Collections.Generic;
using System;
using Tcc.Units;

namespace Tcc.Buffs
{
    public abstract class BuffFromEnemy: Expirable
    {
        public BuffFromEnemy(Guid id, Timestamp expiryTime): base(id, expiryTime)
        {
        }

        public abstract void AddToUnit(Unit unit, List<BuffFromEnemy> buffs);

        public abstract Stats.Stats GetModifier(Enemy.Enemy enemy, Stats.Types type);
    }
}