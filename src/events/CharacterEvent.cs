using System.Collections.Generic;
using System;

namespace Tcc.Events
{
    public class CharacterEvent
    {
        readonly Func<Timestamp, List<WorldEvent>> worldEventGenerator;

        public CharacterEvent(Timestamp timestamp, Func<Timestamp, List<WorldEvent>> worldEventGenerator)
        {
            this.Timestamp = timestamp;
            this.worldEventGenerator = worldEventGenerator;
        }

        public Timestamp Timestamp { get; }

        public List<WorldEvent> GetWorldEvents() => worldEventGenerator(Timestamp);
    }
}
