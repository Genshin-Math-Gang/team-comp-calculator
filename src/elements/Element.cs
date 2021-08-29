namespace Tcc.Elements
{
    public enum Element
    {
        PHYSICAL = 0b1,
        PYRO = 0b10,
        HYDRO = 0b100,
        CRYO = 0b1000,
        ELECTRO = 0b10000,
        ANEMO = 0b100000,
        GEO = 0b1000000,
        DENDRO = 0b10000000,
        ANY = PHYSICAL | PYRO | HYDRO | CRYO | GEO | DENDRO
    }
}