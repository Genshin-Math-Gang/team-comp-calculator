using System;
using Tcc.buffs;
using Tcc.elements;
using Tcc.units;

namespace Tcc.events
{
    public class HitType
    {
        public readonly bool IsAoe;
        public readonly int Bounces;
        public readonly bool SelfBounce;
        public readonly bool IsHeavy;
        public readonly Reaction ReactionType;
        public readonly ICDCreator Creator;
        // TODO: need to figure out how long different attacks take to bounce
        public readonly double Delay;
        public readonly int Gauge;
        public readonly Buff<FirstPassModifier> Debuff;
        private readonly Element? element;
        private InfusionRef _ref;

        public HitType(Element element, bool aoe=true, int bounces=1, bool self=false, bool heavy=false,
                Reaction reaction=Reaction.UNKNOWN, ICDCreator icd=null, double delay=0, int gauge=1, Buff<FirstPassModifier> debuff=null)
        {
            IsAoe = aoe;
            Bounces = bounces;
            SelfBounce = self;
            IsHeavy = heavy;
            ReactionType = reaction;
            Creator = icd;
            Delay = delay;
            Gauge = gauge;
            Debuff = debuff;
            this.element = element;
        }
        
        
        public HitType(InfusionRef element, bool aoe=true, int bounces=1, bool self=false, bool heavy=false,
            Reaction reaction=Reaction.UNKNOWN, ICDCreator icd=null, double delay=0, int gauge=1)
        {
            IsAoe = aoe;
            Bounces = bounces;
            SelfBounce = self;
            IsHeavy = heavy;
            ReactionType = reaction;
            Creator = icd;
            Delay = delay;
            Gauge = gauge;
            _ref = element;
        }

        public Element Element => element ?? _ref.Value;
    }
}