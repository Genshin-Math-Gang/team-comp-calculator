namespace Tcc.Stats
{
    public enum Types
    {
        NONE = 0,
        NORMAL = 0b1,
        CHARGED = 0b10,
        PLUNGE = 0b100,
        SKILL = 0b1000,
        BURST = 0b10000,
        GENERIC = 0b100000,
        TRANSFORMATIVE = 0b1000000,
        SWIRL = 0b10000000,
        ANY = GENERIC | NORMAL | CHARGED | PLUNGE | SKILL | BURST
    }

    public static class TypeHelper
    {
        public static bool IsType(this Types type, Types other) => (type & other) != Types.NONE;
    }
}