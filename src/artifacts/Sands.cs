using System;
using Tcc.stats;

namespace Tcc.artifacts
{
    public class Sands<T>: ArtifactBase<T> where T : ArtifactSet
    {
        public Sands(stats.Stats mainStat) : base(ArtifactSlots.Sands, mainStat)
        {
            if (mainStat is Stats.EnergyRecharge or Stats.AtkPercent or Stats.DefPercent or Stats.HpPercent
                or Stats.ElementalMastery)
            {
                return;
            }

            throw new ArgumentException("not valid main stat for sands");
        }
    }
}