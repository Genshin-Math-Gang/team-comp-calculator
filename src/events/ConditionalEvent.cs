using System;

namespace Tcc.Events
{
    public class ConditionalEvent: WorldEvent
    {
        private Func<object[], bool> condition;
        private object[] param;
        
        public ConditionalEvent(Timestamp timestamp, Action<World> effect, Func<object[], bool> condition, object[] conditionParam,
            String description = null, int priority = 5) : base(timestamp, effect, description, priority)
        {
            this.condition = condition ?? (_ => true);
        }

        public override void Apply(World world)
        {
            if (condition(param))
            {
                base.Apply(world);
            }
        }
    }
}