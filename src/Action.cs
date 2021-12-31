using Tcc.units;

namespace Tcc
{
    public struct Action
    {
        public int Character;
        public Timestamp Timestamp;
        public ActionType ActionType;
        public object[] param;

        public Action(int character, Timestamp timestamp, ActionType type, params object[] p)
        {
            Character = character;
            Timestamp = timestamp;
            ActionType = type;
            param = p;
        }
        
        public Action(int character, Timestamp timestamp, ActionType type) : 
            this(character, timestamp, type, System.Array.Empty<object>()) { }
    }
}