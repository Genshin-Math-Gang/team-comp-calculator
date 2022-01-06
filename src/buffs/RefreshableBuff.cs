using System;
using System.Collections.Generic;
using System.Linq;

namespace Tcc.buffs
{
    public class RefreshableBuff<TModifierT>: Buff<TModifierT>
    {
        readonly double expiryTime;
        readonly int maxStacks;

        public RefreshableBuff(Guid id, double expiryTime, TModifierT modifier, int maxStacks = 1): base(id, modifier)
        {
            this.expiryTime = expiryTime;
            this.maxStacks = maxStacks;
        }

        public override void AddToList(List<Buff<TModifierT>> buffs)
        {
            buffs.Add(this);

            var existingStacks = buffs.Where((buff) => buff.id == this.id);

            var enumerable = existingStacks as Buff<TModifierT>[] ?? existingStacks.ToArray();
            if (enumerable.Length <= maxStacks) return;
            var toRemove = enumerable
                .Cast<RefreshableBuff<TModifierT>>()
                .Aggregate((buff1, buff2) => buff2.expiryTime < buff1.expiryTime ? buff2 : buff1);

            buffs.Remove(toRemove);
        }

        public override bool ShouldRemove(double currentTime) => currentTime >= expiryTime;
    }
}