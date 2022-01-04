using System;
using System.Collections.Generic;
using System.Linq;

namespace Tcc.buffs
{
    public class PermanentBuff<ModifierT>: Buff<ModifierT>
    {
        public PermanentBuff(Guid id, ModifierT modifier)
            : base(id, modifier)
        {
        }

        public override void AddToList(List<Buff<ModifierT>> buffs)
        {
            if (buffs.Any((buff) => buff.id == this.id))
            {
                throw new InvalidBuffException("Cannot duplicate permanent buff");
            }

            buffs.Add(this);
        }

        public override bool ShouldRemove(double currentTime) => false;
    }
}