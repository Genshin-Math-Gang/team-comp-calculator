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
        ANY = NORMAL | CHARGED | PLUNGE | SKILL | BURST
    }

    public static class TypeHelper
    {
        public static bool IsType(this Types type, Types other) => (type & other) != Types.NONE;
    }
}