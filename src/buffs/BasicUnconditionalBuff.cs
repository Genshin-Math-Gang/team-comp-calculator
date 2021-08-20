using System.Collections.Generic;
using System.Linq;
using System;
using Tcc.Units;

namespace Tcc.Buffs
{
    public class BasicUnconditionalBuff: UnconditionalBuff
    {
        readonly Stats.Types type;
        readonly Stats.Stats modifier;

        public BasicUnconditionalBuff(Guid id, Stats.Stats modifier, Stats.Types type = Stats.Types.ANY): base(id, Expirable.Never)
        {
            this.type = type;
            this.modifier = modifier;
        }

        public override void AddToUnit(Unit unit, List<UnconditionalBuff> buffs)
        {
            if(buffs.Any((buff) => buff.Id == this.Id))
            {
                throw new InvalidBuffException("Cannot duplicate permanent buff");
            }

            buffs.Add(this);
        }

        public override Stats.Stats GetModifier(Stats.Types type)
        {
            return (this.type & type) != Stats.Types.NONE ? modifier : new Stats.Stats();
        }
    }
}