using System.Linq;
using System.Collections.Generic;
using Tcc.Stats;
using Tcc.Events;
using Tcc.Buffs;
using System;

namespace Tcc.Units
{
    public class Unit
    {
        protected readonly int constellationLevel;

        private readonly Stats.Stats stats;
        private readonly Dictionary<Types, Stats.Stats> modifiers = new Dictionary<Types, Stats.Stats>
        {
            {Types.NORMAL, new Stats.Stats()},
            {Types.CHARGED, new Stats.Stats()},
            {Types.PLUNGE, new Stats.Stats()},
            {Types.SKILL, new Stats.Stats()},
            {Types.BURST, new Stats.Stats()} 
        };

        private readonly List<BuffFromUnit> buffsFromUnit = new List<BuffFromUnit>();
        private readonly List<BuffFromEnemy> buffsFromEnemy = new List<BuffFromEnemy>();

        protected Unit(int constellationLevel, Stats.Stats stats, Stats.Stats burst, Stats.Stats skill, Stats.Stats normal, Stats.Stats charged, Stats.Stats plunge)
        {
            this.constellationLevel = constellationLevel;

            this.stats = stats;

            this.modifiers[Types.NORMAL] = normal;
            this.modifiers[Types.BURST] = burst;
            this.modifiers[Types.CHARGED] = charged;
            this.modifiers[Types.PLUNGE] = plunge;
            this.modifiers[Types.SKILL] = skill;
        }

        public List<WorldEvent> switchUnit(Timestamp timestamp)
        {
            return new List<WorldEvent> { new SwitchUnit(timestamp, this) };
        }

        protected Stats.Stats getStats(Types type, Timestamp timestamp)
        {
            buffsFromUnit.RemoveAll((buff) => buff.HasExpired(timestamp));

            Stats.Stats requested = modifiers[type] + stats;

            foreach (var buff in buffsFromUnit)
            {
                requested += buff.GetModifier(this, type);
            }

            return requested;
        }

        protected Func<Timestamp, Stats.Stats> getStats(Types type)
        {
            return (timestamp) => getStats(type, timestamp);
        }

        public void AddBuff(BuffFromUnit buff)
        {
            buff.AddToUnit(this, this.buffsFromUnit);
        }

        public void AddBuff(BuffFromEnemy buff)
        {
            buff.AddToUnit(this, this.buffsFromEnemy);
        }
    }
}