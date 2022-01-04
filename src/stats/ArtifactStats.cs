using System.Collections.Generic;
using System.Linq;
using Tcc.artifacts;

namespace Tcc.stats
{
    public class ArtifactStats: StatsPage
    {
        private ArtifactBase[] artifactList;

        // need to add logic for determining set effects

        public ArtifactBase this[ArtifactSlots slot] => artifactList[(int) slot];

        public StatsPage Stats;

        public ArtifactStats(Flower f=null, Feather ff=null, Sands s=null, Goblet g=null, Circlet c=null)
        {
            artifactList = new ArtifactBase[] {f, ff, s, g, c};
            Stats = new StatsPage();
            // TODO: NEED TO MAKE IT SO IF ARTIFACTS CHANGE THIS CHANGES
            foreach (var artifact in artifactList)
            {
                if (artifact is not null) Stats.Add(artifact.ArtifactStats);
            }

        }


    }
    
    
}