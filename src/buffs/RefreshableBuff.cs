using System.Collections.Generic;
using System.Linq;
using System;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Buffs
{
    public class RefreshableBuff: BuffFromUnit
    {
        readonly Stats.Types type;
        readonly Stats.Stats modifier;
        readonly Timestamp expiryTime;
        readonly int maxStacks;

        public RefreshableBuff(Guid id, Timestamp expiryTime, Stats.Stats modifier, Stats.Types type = Stats.Types.ANY, int maxStacks = 1): base(id, expiryTime)
        {
            this.type = type;
            this.modifier = modifier;
            this.expiryTime = expiryTime;
            this.maxStacks = maxStacks;
        }

        public override void AddToUnit(Unit unit, List<BuffFromUnit> buffs)
        {
            buffs.Add(this);

            var existingStacks = buffs
                .Where((buff) => buff.Id == this.Id);

            if(existingStacks.Count() > maxStacks)
            {
                var toRemove = existingStacks
                    .Cast<RefreshableBuff>()
                    .Aggregate((buff1, buff2) => buff2.expiryTime < buff1.expiryTime ? buff2 : buff1);

                buffs.Remove(toRemove);
            }
        }

        public override Stats.Stats GetModifier(Unit unit, Stats.Types type)
        {
            return this.type.IsType(type) ? modifier : new Stats.Stats();
        }
    }
}