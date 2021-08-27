using System.Collections.Generic;
using System;
using Tcc.Stats;

namespace Tcc.Buffs
{
    public class Expirable
    {
        readonly Timestamp expiryTime;

        public Expirable(Guid id, Timestamp expiryTime)
        {
            this.Id = id;
            this.expiryTime = expiryTime;
        }

        public Guid Id { get; }

        public bool HasExpired(Timestamp currentTime) => expiryTime != null && expiryTime <= currentTime;

        public const Timestamp Never = null;
    }
}
