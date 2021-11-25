using System;
using System.Collections.Generic;

namespace Tcc.events
{
    public class CharacterEvent
    {
        readonly Func<Timestamp, object[], List<WorldEvent>> worldEventGenerator;
        private object[] param;

        public CharacterEvent(Timestamp timestamp, Func<Timestamp, object[], List<WorldEvent>> worldEventGenerator, params object[] param)
        {
            this.Timestamp = timestamp;
            this.param = param;
            this.worldEventGenerator = worldEventGenerator;
        }
        
        public CharacterEvent(Timestamp timestamp, Func<Timestamp, List<WorldEvent>> worldEventGenerator)
        {
            this.Timestamp = timestamp;
            // this is sus as hell but i think it works
            this.worldEventGenerator = (t, p) => worldEventGenerator(t);
        }
        
        public CharacterEvent(Timestamp timestamp, Func<Timestamp, object[], List<WorldEvent>> worldEventGenerator, World world)
        {
            this.Timestamp = timestamp;
            param = new[] {world};
            // this is sus as hell but i think it works
            this.worldEventGenerator = (t, p) => worldEventGenerator(t, p);
        }
        

        public Timestamp Timestamp { get; }

        public List<WorldEvent> GetWorldEvents() => worldEventGenerator(Timestamp, param);
    }
}
