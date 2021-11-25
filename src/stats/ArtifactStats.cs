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

        public StatsPage Stats
        {
            get
            {
                var result = new StatsPage();

                return artifactList.Where(slot => slot is not null).Aggregate(result, 
                    (current, slot) => current + slot.ArtifactStats) ?? new StatsPage();
            }
        }

        public ArtifactStats(Flower f=null, Feather ff=null, Sands s=null, Goblet g=null, Circlet c=null)
        {
            artifactList = new ArtifactBase[] {f, ff, s, g, c};

        }
    }
    
    
}