using Tcc.stats;

namespace Tcc.artifacts
{
    public class Flower<T>: ArtifactBase<T> where T : ArtifactSet
    {
        public Flower() : base(ArtifactSlots.Flower, Stats.HpFlat) {}
    }
}