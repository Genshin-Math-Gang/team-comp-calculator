using System;

namespace Tcc.buffs
{
    public class InvalidBuffException: Exception
    {
        public InvalidBuffException(string reason): base(reason)
        {
        }
    }
}