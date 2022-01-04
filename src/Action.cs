using Tcc.units;

namespace Tcc
{
    public struct Action
    {
        public int Character;
        public double Timestamp;
        public ActionType ActionType;
        public object[] param;

        public Action(int character, double timestamp, ActionType type, params object[] p)
        {
            Character = character;
            Timestamp = timestamp;
            ActionType = type;
            param = p;
        }
        
        public Action(int character, double timestamp, ActionType type) : 
            this(character, timestamp, type, System.Array.Empty<object>()) { }
    }
}