using System;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using Tcc.events;

namespace Tcc
{
    public class WorldEventQueue
    {
        private List<WorldEvent> _events;

        public WorldEventQueue(List<CharacterEvent> characterEvents)
        {
            _events = characterEvents
                .SelectMany((characterEvent) => characterEvent.GetWorldEvents())
                .ToList();
            _events.Sort();
        }
        
        public WorldEventQueue()
        {
            _events = new List<WorldEvent>();
        }

        public void Add(WorldEvent worldEvent)
        {
            int index = _events.BinarySearch(worldEvent);
            if (index < 0)
            {
                _events.Insert(-index-1, worldEvent);
            }
            else
            {
                _events.Insert(index, worldEvent);
            }
        }

        public WorldEvent DeQueue()
        {
            var temp = _events[0];
            _events.RemoveAt(0);
            return temp;
        }

        public bool IsEmpty()
        {
            return _events.Any();
        }
    }
}