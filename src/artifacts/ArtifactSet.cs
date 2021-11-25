using Tcc.units;

namespace Tcc.artifacts
{
    public abstract class ArtifactSet
    {
        public abstract void Add2pc(World world, Unit unit);
        public abstract void Add4pc(World world, Unit unit);
    }
}