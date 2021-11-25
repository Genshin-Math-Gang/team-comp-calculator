using Tcc.elements;

namespace Tcc.events
{
    public struct HitType
    {
        public readonly bool IsAoe;
        public readonly int Bounces;
        public readonly bool SelfBounce;
        public readonly bool IsHeavy;
        public readonly Reaction ReactionType;
        public readonly ICDCreator Creator;
        // TODO: need to figure out how long different attacks take to bounce
        public readonly Timestamp Delay;
        public readonly int Gauge;

        public HitType(bool aoe=true, int bounces=0, bool self=false, bool heavy=false, Reaction reaction=Reaction.UNKNOWN, 
            ICDCreator icd=null, Timestamp delay=null, int gauge=1)
        {
            IsAoe = aoe;
            Bounces = bounces;
            SelfBounce = self;
            IsHeavy = heavy;
            ReactionType = reaction;
            Creator = icd;
            Delay = delay ?? new Timestamp(0.1);
            Gauge = gauge;
        }
    }
}