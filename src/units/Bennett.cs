using System.Collections.Generic;
using Tcc.Events;
using Tcc.Stats;

namespace Tcc.Units
{
    public class Bennett: Unit
    {
        public Bennett(int constellationLevel): base(constellationLevel, 
        normal:new Stats.Stats(
            new double[] {0.8806,0.8449,1.0795,1.11798,1.3209},10876,0,0,0,225,0.466,311,0.5,1
        ),
        charged: new Stats.Stats(
            new double[] {1.105+1.202},10876,0,0,0,225,0.466,311,0.5,1
        ),
        plunge: new Stats.Stats(
            new double[] {1.2638,2.527,3.1564},10876,0,0,0,225,0.466,311,0.5,1
        ),
        skill: new Stats.Stats(
            new double[] {2.4768,1.512+1.656,1.584+1.728,2.376},10876,0,0,0,225,0.466,311,0.5,1
        ),
        burst: new Stats.Stats(
            new double[] {4.1904,0.108,1.008},10876,0,0,0,225,0.466,311,0.5,1
        ))
        {
        }

        public List<WorldEvent> Burst(double timestamp)
        {
            Stats.Stats buff = new Stats.Stats(new double[] {-1},0,0,0,0,0,0, getStats(Types.BURST).BaseAttack*getStats(Types.BURST).MV[2],0,0);

            List<WorldEvent> temp = new List<WorldEvent> {
                new Hit(timestamp, () => getStats(Types.BURST) + buff, 0, this, "bennett burst"),
            };

            for(int i=1; i < 8; i++)
            {
                temp.Add(new RemoveBuffGlobal(timestamp + i*2, "Fantastic Voyage", Types.EVERYTHING ,"bennett Q has been removed globally"));
                temp.Add(new AddBuffOnField(timestamp + i*2, buff, "Fantastic Voyage", Types.EVERYTHING, "bennett Q added on on-field unit"));
            }

            temp.Add(new RemoveBuffGlobal(timestamp + 14, "Fantastic Voyage", Types.EVERYTHING ,"bennett Q has been removed globally"));

            return temp;
        }
       
        public override string ToString()
        {
            return "Bennett";
        }
    }
}