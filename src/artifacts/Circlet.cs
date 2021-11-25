using System;
using Tcc.stats;

namespace Tcc.artifacts

{
    public class Circlet<T>: ArtifactBase<T> where T : ArtifactSet
    {
        public Circlet(Stats mainStat) : base(ArtifactSlots.Circlet, mainStat)
        {
            if (mainStat is Stats.AtkPercent or Stats.DefPercent or Stats.HpPercent
                or Stats.ElementalMastery or Stats.CritRate or Stats.CritDamage or Stats.HealingBonus)
            {
                return;
            }

            throw new ArgumentException("not valid main stat for Circlet");
        }
    }
}