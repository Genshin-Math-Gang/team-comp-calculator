using System;
using Tcc.stats;

namespace Tcc.artifacts
{
    public class Sands: ArtifactBase
    {
        public Sands(stats.Stats mainStat, ArtifactSet artifactSet=null) : base(ArtifactSlots.Sands, mainStat, artifactSet)
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