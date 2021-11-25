using System;
using Tcc.stats;

namespace Tcc.artifacts
{
    public class Goblet<T>: ArtifactBase<T> where T : ArtifactSet
    {
        public Goblet(Stats mainStat) : base(ArtifactSlots.Goblet, mainStat)
        {
            if (mainStat is Stats.AtkPercent or Stats.DefPercent or Stats.HpPercent or Stats.ElementalMastery 
                or Stats.PyroDamageBonus or Stats.HydroDamageBonus or Stats.ElectroDamageBonus or Stats.CryoDamageBonus
                or Stats.DendroDamageBonus or Stats.AnemoDamageBonus or Stats.GeoDamageBonus or Stats.PhysicalDamageBonus)
            {
                return;
            }

            throw new ArgumentException("not valid main stat for Goblet");
        }
    }
}