using System;
using System.Reflection.Emit;
using Tcc.Events;
using System.Collections.Generic;
using System.Linq;

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

        public void Add(WorldEvent worldEvent)
        {
            int index = Math.Abs(_events.BinarySearch(worldEvent));
            _events.Insert(index, worldEvent);
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