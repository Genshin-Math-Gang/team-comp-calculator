using System;
using System.ComponentModel;

namespace Tcc.Events
{
    public class WorldEvent: IComparable<WorldEvent>
    {
        readonly Action<World> effect;
        private readonly Action<bool, object[]> condition;
        public String Descrption;
        
        // TODO: maybe this should be its own class but i am lazy rn
        private int priority;

        public WorldEvent(Timestamp timestamp, Action<World> effect, String description=null, int priority=5, 
            Action<bool, object[]> condition=null)
        {
            this.Timestamp = timestamp;
            this.effect = effect;
            this.Descrption = description;
            this.priority = priority;
            
        }

        public int CompareTo(WorldEvent other)
        {
            int temp = Timestamp.CompareTo(other.Timestamp);
            if (temp != 0)
            {
                return temp;
            }

            return this.priority.CompareTo(other.priority);
        }

        public Timestamp Timestamp { get; }

        public virtual void Apply(World world) => effect(world);

        public override string ToString()
        {
            return $"{Descrption ?? "Event"} at {Timestamp}";
        }
    }
    
}
