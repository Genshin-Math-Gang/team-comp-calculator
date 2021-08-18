using System.Collections.Generic;
using System.Linq;
using System;
using Tcc.Units;

namespace Tcc.Buffs
{
    public class BasicBuffFromUnit: BuffFromUnit
    {
        readonly Func<Unit, bool> condition;
        readonly Stats.Types type;
        readonly Stats.Stats modifier;

        public BasicBuffFromUnit(Guid id, Stats.Stats modifier, Stats.Types type = Stats.Types.EVERYTHING, Func<Unit, bool> condition = null): base(id, Expirable.Never)
        {
            this.condition = condition ?? ((_) => true);
            this.type = type;
            this.modifier = modifier;
        }

        public override void AddToUnit(Unit unit, List<BuffFromUnit> buffs)
        {
            if(buffs.Any((buff) => buff.Id == this.Id))
            {
                throw new InvalidBuffException("Cannot duplicate permanent buff");
            }

            buffs.Add(this);
        }

        public override Stats.Stats GetModifier(Unit unit, Stats.Types type)
        {
            return this.type == type && condition(unit) ? modifier : new Stats.Stats();
        }
    }
}