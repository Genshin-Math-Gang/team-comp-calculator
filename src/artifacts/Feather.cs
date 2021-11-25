using Tcc.stats;

namespace Tcc.artifacts
{
    public class Feather: ArtifactBase
    {
        public Feather(ArtifactSet artifactSet=null) : base(ArtifactSlots.Feather, Stats.AtkFlat, artifactSet) {}
    }
}