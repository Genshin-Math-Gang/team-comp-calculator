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

        public HitType(bool aoe, int bounces, bool self, bool heavy=false, Reaction reaction=Reaction.UNKNOWN, ICDCreator icd=null)
        {
            IsAoe = aoe;
            Bounces = bounces;
            SelfBounce = self;
            IsHeavy = heavy;
            ReactionType = reaction;
            Creator = icd;
        }
    }
}