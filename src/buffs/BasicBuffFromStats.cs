using System.Collections.Generic;
using System.Linq;
using System;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs
{
    public class BasicBuffFromStats: BuffFromStats
    {
        readonly Func<Unit, Stats.Stats, Timestamp, bool> condition;
        readonly Func<Unit, Stats.Stats, Timestamp, Stats.Stats> modifier;

        public BasicBuffFromStats(Guid id, Stats.Stats modifier, Stats.Types type = Stats.Types.ANY, Func<Unit, Stats.Stats, Timestamp, bool> condition = null)
            : this(id, (_, _, _) => modifier, type, condition)
        {
        }

        public BasicBuffFromStats(Guid id, Func<Unit, Stats.Stats, Timestamp, Stats.Stats> modifier, Stats.Types type = Stats.Types.ANY, Func<Unit, Stats.Stats, Timestamp, bool> condition = null, Timestamp expiryTime = Expirable.Never): base(id, expiryTime)
        {
            this.condition = condition ?? ((_, _, _) => true);
            this.type = type;
            this.modifier = modifier;
        }

        public override void AddToUnit(Unit unit, List<BuffFromStats> buffs)
        {

            buffs.RemoveAll((buff) => buff.Id == this.Id);

            buffs.Add(this);
        }

        public override Stats.Stats GetModifier(Unit unit, Stats.Stats firstPassStats, Timestamp timestamp, Stats.Types type)
        {
            return this.type.IsType(type) && condition(unit, firstPassStats, timestamp) ? modifier(unit, firstPassStats, timestamp) : new Stats.Stats();
        }
    }
}