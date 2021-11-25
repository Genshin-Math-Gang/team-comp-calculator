using System;
using System.Collections.Generic;

namespace Tcc.buffs
{
    public abstract class Buff<ModifierT>
    {
        public readonly Guid id;

        public Buff(Guid id, ModifierT modifier)
        {
            this.id = id;
            this.GetModifier = modifier;
        }

        public ModifierT GetModifier { get; }

        public abstract void AddToList(List<Buff<ModifierT>> buffs);
        public abstract bool ShouldRemove(Timestamp currentTime);
    }
}