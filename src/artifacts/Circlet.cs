using System;
using Tcc.stats;

namespace Tcc.artifacts

{
    public class Circlet: ArtifactBase
    {
        public Circlet(Stats mainStat, ArtifactSet artifactSet= null) : base(ArtifactSlots.Circlet, mainStat, artifactSet)
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