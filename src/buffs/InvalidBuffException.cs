using System;

namespace Tcc.Buffs
{
    public class InvalidBuffException: Exception
    {
        public InvalidBuffException(string reason): base(reason)
        {
        }
    }
}