namespace Tcc.Elements
{
    public static class Reaction
    {
        public const int MELT = 1;
        public const int VAPORIZE = 2;
        public const int OVERLOADED = -1;
        public const int FREEZE = -2;
        public const int ELECTROCHARGED = -3;
        public const int SUPERCONDUCT = -4;
        public const int SWIRL_PYRO = -5;
        public const int SWIRL_HYDRO = -6;
        public const int SWIRL_CRYO = -7;
        public const int SWIRL_ELECTRO = -8;
        public const int CRYSTALIZE_PYRO = -9;
        public const int CRYSTALIZE_HYDRO = -10;
        public const int CRYSTALIZE_CRYO = -11;
        public const int CRYSTALIZE_ELECTRO = -12;
        public const int BURNING = -13;
        public const int SHATTERED = -14;
        public const int NONE = 0;

        public static double ReactionMultiplier (double reaction)
        {
            switch (reaction)
            {
                case Reaction.BURNING: return 0.25;
                case Reaction.ELECTROCHARGED: return 1.2;
                case Reaction.SUPERCONDUCT: return 0.5;
                case Reaction.SHATTERED: return 1.5;
                case Reaction.OVERLOADED: return 2;
            }

            return Reaction.NONE;
        }
    }
}