using System.Collections.Generic;
using System;

namespace Tcc.Events
{
    public class CharacterEvent
    {
        readonly Func<double, List<WorldEvent>> worldEventGenerator;

        public CharacterEvent(double timestamp, Func<double, List<WorldEvent>> worldEventGenerator)
        {
            this.Timestamp = timestamp;
            this.worldEventGenerator = worldEventGenerator;
        }

        public double Timestamp { get; }

        public List<WorldEvent> GetWorldEvents() => worldEventGenerator(Timestamp);
    }
}
