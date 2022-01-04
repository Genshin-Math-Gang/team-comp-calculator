using System;
using Tcc.elements;
using Tcc.enemy;
using Tcc.stats;
using Tcc.units;

namespace Tcc.events
{
    // this is a kinda scuffed way of implementing things but whatever since it should work
    public class ConditionalHit: Hit
    {
        private Func<object[], bool> condition;
        private object[] param;
        
        public ConditionalHit(
            double timestamp, int mvIndex, Func<double, SecondPassStatsPage> stats,
            Unit unit, Types type, HitType hitType, Func<object[], bool> condition, object[] conditionParam,
            string description = "", Enemy target = null): 
            base(timestamp, mvIndex, stats, unit, type, hitType, description, target)
        {
            this.condition = condition;
            this.param = conditionParam;
        }
        
        public ConditionalHit(
            double timestamp, Func<Element> element, int mvIndex, Func<double, SecondPassStatsPage> stats,
            Unit unit, Types type, HitType hitType, Func<object[], bool> condition, object[] conditionParam,
            string description = "", Enemy target = null): 
            base(timestamp, mvIndex, stats, unit, type, hitType, description, target)
        {
            this.condition = condition;
            this.param = conditionParam;
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