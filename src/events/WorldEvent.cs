using System;

namespace Tcc.events
{
    public class WorldEvent: IComparable<WorldEvent>
    {
        readonly Action<World> effect;
        private readonly Action<bool, object[]> condition;
        public String Descrption;
        
        // TODO: maybe this should be its own class but i am lazy rn
        private int priority;

        public WorldEvent(double timestamp, Action<World> effect, String description=null, int priority=5, 
            Action<bool, object[]> condition=null)
        {
            this.Timestamp = timestamp;
            this.effect = effect;
            this.Descrption = description;
            this.priority = priority;
            
        }

        public int CompareTo(WorldEvent other)
        {
            // TODO: check this is fine
            double temp = Timestamp - other.Timestamp;
            return temp switch
            {
                <0 => -1,
                >0 => 1,
                _ => priority.CompareTo(other.priority)
            };
        }

        public double Timestamp { get; }

        public virtual void Apply(World world) => effect(world);

        public override string ToString()
        {
            return $"{Descrption ?? "Event"} at {Timestamp}";
        }
    }
    
}
