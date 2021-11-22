﻿using System;
using Tcc.Elements;
using Tcc.Stats;

namespace Tcc.Events
{
    // this is a kinda scuffed way of implementing things but whatever since it should work
    public class ConditionalHit: Hit
    {
        private Func<object[], bool> condition;
        private object[] param;
        
        public ConditionalHit(
            Timestamp timestamp, Element element, int mvIndex, Func<Timestamp, SecondPassStatsPage> stats,
            Units.Unit unit, Types type, HitType hitType, Func<object[], bool> condition, object[] conditionParam,
            string description = "", Enemy.Enemy target = null): 
            base(timestamp, element, mvIndex, stats, unit, type, hitType, description, target)
        {
            this.condition = condition;
            this.param = conditionParam;
        }
        
        public ConditionalHit(
            Timestamp timestamp, Func<Element> element, int mvIndex, Func<Timestamp, SecondPassStatsPage> stats,
            Units.Unit unit, Types type, HitType hitType, Func<object[], bool> condition, object[] conditionParam,
            string description = "", Enemy.Enemy target = null): 
            base(timestamp, element, mvIndex, stats, unit, type, hitType, description, target)
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