using System;
using Tcc.Buffs;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Elements
{
    public class AddBuff<ModifierT>: WorldEvent
    {
        // TODO: priority is hacked together and subject to change
        public AddBuff(Timestamp timestamp, Buff<FirstPassModifier> buff, StatObject st): 
            base(timestamp, _ => st.AddBuff(buff), priority:1)
        {
            
        }
        
        public AddBuff(Timestamp timestamp, Buff<SecondPassModifier> buff, StatObject st): 
            base(timestamp, _ => st.AddBuff(buff), priority:1)
        {
            
        }
    }
}