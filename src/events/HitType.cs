using Tcc.Elements;

namespace Tcc.Events
{
    public struct HitType
    {
        public bool IsAoe;
        public int Bounces;
        public bool SelfBounce;
        public bool IsHeavy;
        public Reaction ReactionType;
        public ICDCreator Creator;
        // TODO: need to figure out how long different attacks take to bounce
        public Timestamp Delay;

        public HitType(bool aoe, int bounces, bool self, bool heavy=false, Reaction reaction=Reaction.UNKNOWN, 
            ICDCreator icd=null, Timestamp delay=null)
        {
            IsAoe = aoe;
            Bounces = bounces;
            SelfBounce = self;
            IsHeavy = heavy;
            ReactionType = reaction;
            Creator = icd;
            Delay = delay ?? new Timestamp(0.1);
        }
    }
}