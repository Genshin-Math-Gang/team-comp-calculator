namespace Tcc.Elements
{
    public static class Reaction
    {
        public const double MELT = 1;
        public const double VAPORIZE = 2;
        public const double OVERLOADED = -1;
        public const double FREEZE = -2;
        public const double ELECTROCHARGED = -3;
        public const double SUPERCONDUCT = -4;
        public const double SWIRL_PYRO = -5;
        public const double SWIRL_HYDRO = -6;
        public const double SWIRL_CRYO = -7;
        public const double SWIRL_ELECTRO = -8;
        public const double CRYSTALIZE_PYRO = -9;
        public const double CRYSTALIZE_HYDRO = -10;
        public const double CRYSTALIZE_CRYO = -11;
        public const double CRYSTALIZE_ELECTRO = -12;
        public const double BURNING = -13;
        public const double SHATTERED = -14;
        public const double NONE = 0;

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