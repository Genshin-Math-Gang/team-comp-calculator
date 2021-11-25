using Tcc.stats;

namespace Tcc.artifacts
{
    public class Flower: ArtifactBase
    {
        public Flower(ArtifactSet artifactSet=null) : base(ArtifactSlots.Flower, Stats.HpFlat, artifactSet) {}
    }
}