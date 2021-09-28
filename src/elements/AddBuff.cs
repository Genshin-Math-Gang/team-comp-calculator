using Tcc.Buffs;
using Tcc.Events;
using Tcc.Units;

namespace Tcc.Elements
{
    public class AddBuff<ModifierT>: WorldEvent
    {
        public AddBuff(Timestamp timestamp, Buff<FirstPassModifier> buff, StatObject st): 
            base(timestamp, world => st.AddBuff(buff))
        {
            
        }
    }
}