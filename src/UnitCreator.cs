using Tcc.units;

namespace Tcc
{
    public struct UnitCreator
    {
        public Character Character;
        public string Level;
        public int Cons;
        public int AutoLevel;
        public int SkillLevel;
        public int BurstLevel;

        public UnitCreator(Character character, string level="90", int cons=0, int aa=6, int skill=6, int burst=6)
        {
            Character = character;
            Level = level;
            Cons = cons;
            AutoLevel = aa;
            SkillLevel = skill;
            BurstLevel = burst;
        }
        
        public static implicit operator UnitCreator(Character character) => new(character);
    }
}