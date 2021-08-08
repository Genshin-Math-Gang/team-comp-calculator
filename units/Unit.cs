namespace Tcc.Units
{
    public class Unit
    {
        readonly int constellationLevel;
        readonly UnitStats baseStats;

        protected Unit(int constellationLevel, UnitStats baseStats)
        {
            this.constellationLevel = constellationLevel;
            this.baseStats = baseStats;
        }


    }
}