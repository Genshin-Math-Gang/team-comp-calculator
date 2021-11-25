namespace Tcc.elements
{
    public enum Reaction
    {
        UNKNOWN = -1,
        NONE = 0,
        OVERLOADED = 1,
        FREEZE = 2,
        ELECTROCHARGED = 3,
        SUPERCONDUCT = 4,
        BURNING = 5,
        SHATTERED = 6,
        SWIRL_PYRO = 7,
        SWIRL_HYDRO = 8,
        SWIRL_CRYO = 9,
        SWIRL_ELECTRO = 10,
        CRYSTALLIZE_PYRO = 11,
        CRYSTALLIZE_HYDRO = 12,
        CRYSTALLIZE_CRYO = 13,
        CRYSTALLIZE_ELECTRO = 14,
        MELT = 16,
        VAPORIZE = 17
    }

    // this whole thing is scuffed but whatever
    public static class ReactionTypes
    {
        public static bool IsSwirl(Reaction reaction)
        {
            int value = (int) reaction;
            return value is < 11 and > 6;
        }
        
        public static bool IsTransformative(Reaction reaction)
        {
            int value = (int) reaction;
            return value is < 15 and > 0;
        }
    }
    
    
    
}