using System.Collections.Generic;
using System.Linq;
using System;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs
{
    public class BasicBuffFromEnemy: BuffFromEnemy
    {
        readonly Func<Enemy.Enemy, bool> condition;
        readonly Stats.Stats modifier;

        public BasicBuffFromEnemy(Guid id, Func<Enemy.Enemy, bool> condition, Stats.Stats modifier, Stats.Types type = Stats.Types.ANY): base(id, Expirable.Never)
        {
            this.condition = condition;
            this.type = type;
            this.modifier = modifier;
        }

        public override void AddToUnit(Unit unit, List<BuffFromEnemy> buffs)
        {
            if(buffs.Any((buff) => buff.Id == this.Id))
            {
                throw new InvalidBuffException("Cannot duplicate permanent buff");
            }

            buffs.Add(this);
        }

        public override Stats.Stats GetModifier(Enemy.Enemy enemy, Stats.Types type)
        {
            return this.type.IsType(type) && condition(enemy) ? modifier : new Stats.Stats();
        }
    }
}