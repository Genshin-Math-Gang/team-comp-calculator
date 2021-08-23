using System;
using System.Linq;
using System.Collections.Generic;
using Tcc.Buffs;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Weapons;

namespace Tcc.Units
{
    public class Unit
    {
        protected readonly int constellationLevel;
        protected readonly int burstEnergyCost;

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
        private readonly List<BuffFromStats> buffsFromStats = new List<BuffFromStats>();
        private readonly List<BuffFromEnemy> buffsFromEnemy = new List<BuffFromEnemy>();

        public event EventHandler<Timestamp> skillActivatedHook;
        public event EventHandler<Timestamp> burstActivatedHook;
        public event EventHandler<(Timestamp timestamp, Reaction reaction)> triggeredReactionHook; // TODO Not fired by anything
        public event EventHandler<(Timestamp timestamp, Element? element)> particleCollectedHook; // TODO Not fired by anything

        protected Unit(int constellationLevel, Element element, int burstEnergyCost, Stats.Stats stats, Stats.Stats burst, Stats.Stats skill, Stats.Stats normal, Stats.Stats charged, Stats.Stats plunge)
        {
            this.constellationLevel = constellationLevel;
            this.Element = element;

            this.burstEnergyCost = burstEnergyCost;
            this.CurrentEnergy = burstEnergyCost;

            this.stats = stats;

            this.modifiers[Types.NORMAL] = normal;
            this.modifiers[Types.BURST] = burst;
            this.modifiers[Types.CHARGED] = charged;
            this.modifiers[Types.PLUNGE] = plunge;
            this.modifiers[Types.SKILL] = skill;
        }

        public Element Element { get; }
        public Weapon Weapon { get; set; }

        public double CurrentHp { get; }
        public double CurrentEnergy { get; private set; }
        public bool IsShielded => throw new NotImplementedException();

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
            buffsFromUnit.RemoveAll((buff) => buff.HasExpired(timestamp));
            buffsFromStats.RemoveAll((buff) => buff.HasExpired(timestamp));

            var firstPassStats = modifiers[type] + stats;
            foreach(var buff in buffsFromUnit) firstPassStats += buff.GetModifier(this, type);

            var result = firstPassStats;
            foreach(var buff in buffsFromStats) result += buff.GetModifier(this, firstPassStats, type);

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

        public void AddBuff(BuffFromUnit buff)
        {
            buff.AddToUnit(this, this.buffsFromUnit);
        }

        public void AddBuff(BuffFromStats buff)
        {
            buff.AddToUnit(this, this.buffsFromStats);
        }

        public void AddBuff(BuffFromEnemy buff)
        {
            buff.AddToUnit(this, this.buffsFromEnemy);
        }

        public int GetBuffCount(Guid id)
        {
            return buffsFromUnit.Count((buff) => buff.Id == id)
                + buffsFromStats.Count((buff) => buff.Id == id)
                + buffsFromEnemy.Count((buff) => buff.Id == id);
        }

        public void GiveEnergy(int energy) => CurrentEnergy = Math.Min(CurrentEnergy + energy, burstEnergyCost);
        public void LoseEnergy(int energy) => CurrentEnergy = Math.Max(CurrentEnergy - energy, 0);

        protected WorldEvent SkillActivated(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (world) => skillActivatedHook?.Invoke(this, timestamp));
        }

        protected WorldEvent BurstActivated(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (world) => burstActivatedHook?.Invoke(this, timestamp));
        }

        protected WorldEvent TriggeredReaction(Timestamp timestamp, Reaction reaction)
        {
            return new WorldEvent(timestamp, (world) => triggeredReactionHook?.Invoke(this, (timestamp, reaction)));
        }

        protected WorldEvent ParticleCollected(Timestamp timestamp, Element? element)
        {
            return new WorldEvent(timestamp, (world) => particleCollectedHook?.Invoke(this, (timestamp, element)));
        }
    }
}