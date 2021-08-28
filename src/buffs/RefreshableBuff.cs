using System.Collections.Generic;
using System.Linq;
using System;

namespace Tcc.Buffs
{
    public class RefreshableBuff<ModifierT>: Buff<ModifierT>
    {
        readonly Timestamp expiryTime;
        readonly int maxStacks;

        public RefreshableBuff(Guid id, Timestamp expiryTime, ModifierT modifier, int maxStacks = 1): base(id, modifier)
        {
            this.expiryTime = expiryTime;
            this.maxStacks = maxStacks;
        }

        public override void AddToList(List<Buff<ModifierT>> buffs)
        {
            buffs.Add(this);

            var existingStacks = buffs.Where((buff) => buff.id == this.id);

            if(existingStacks.Count() > maxStacks)
            {
                var toRemove = existingStacks
                    .Cast<RefreshableBuff<ModifierT>>()
                    .Aggregate((buff1, buff2) => buff2.expiryTime < buff1.expiryTime ? buff2 : buff1);

                buffs.Remove(toRemove);
            }
        }

        public override bool ShouldRemove(Timestamp currentTime) => currentTime >= expiryTime;
    }
}