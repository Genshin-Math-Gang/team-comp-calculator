using Tcc.stats;

namespace Tcc.artifacts
{
    public class Feather<T>: ArtifactBase<T> where T : ArtifactSet
    {
        public Feather() : base(ArtifactSlots.Feather, Stats.HpFlat) {}
    }
}