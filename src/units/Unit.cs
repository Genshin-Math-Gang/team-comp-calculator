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

        private readonly List<UnconditionalBuff> unconditionalBuffs = new List<UnconditionalBuff>();
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

        public double CurrentHp { get; }

        public List<WorldEvent> SwitchUnit(Timestamp timestamp)
        {
            return new List<WorldEvent> { new SwitchUnit(timestamp, this) };
        }

        protected Stats.Stats GetStats(Types type, Enemy.Enemy enemy, Timestamp timestamp)
        {
            var result = GetStatsFromUnit(type, timestamp);
            result = AddStatsFromEnemy(result, type, enemy, timestamp);

            return result;
        }

        public Stats.Stats GetStatsFromUnit(Types type, Timestamp timestamp)
        {
            unconditionalBuffs.RemoveAll((buff) => buff.HasExpired(timestamp));
            buffsFromUnit.RemoveAll((buff) => buff.HasExpired(timestamp));

            var unconditionalStats = modifiers[type] + stats;
            foreach(var buff in unconditionalBuffs) unconditionalStats += buff.GetModifier(type);

            var result = unconditionalStats;
            foreach(var buff in buffsFromUnit) result += buff.GetModifier(this, unconditionalStats, type);

            return result;
        }

        public Stats.Stats AddStatsFromEnemy(Stats.Stats statsFromUnit, Types type, Enemy.Enemy enemy, Timestamp timestamp)
        {
            buffsFromEnemy.RemoveAll((buff) => buff.HasExpired(timestamp));

            var result = statsFromUnit;

            if(enemy != null)
            {
                foreach(var buff in buffsFromEnemy) result += buff.GetModifier(enemy, type);
            }

            return result;
        }

        protected Func<Enemy.Enemy, Timestamp, Stats.Stats> GetStats(Types type)
        {
            return (enemy, timestamp) => GetStats(type, enemy, timestamp);
        }

        public void AddBuff(UnconditionalBuff buff)
        {
            buff.AddToUnit(this, this.unconditionalBuffs);
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