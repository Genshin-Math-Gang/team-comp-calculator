using System.Collections.Generic;
using Tcc.events;
using Tcc.units;

namespace Tcc
{
    public class ActionList
    {
        public UnitCreator[] characters = new UnitCreator[4];
        public List<Action> eventList;

        public ActionList(UnitCreator u1, UnitCreator u2, UnitCreator u3, UnitCreator u4, List<Action> actions)
        {
            characters[0] = u1;
            characters[1] = u2;
            characters[2] = u3;
            characters[3] = u4;
            eventList = actions;
        }

        
    }
}