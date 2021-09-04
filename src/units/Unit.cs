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
    public abstract class Unit
    {
        protected readonly int constellationLevel;
        protected readonly int burstEnergyCost;
        public readonly WeaponType weaponType;
        public readonly Element element;

        // Base stats
        protected readonly CapacityStats startingCapacityStats;
        protected readonly GeneralStats startingGeneralStats;
        protected readonly Dictionary<Types, AbilityStats> startingAbilityStats = new Dictionary<Types, AbilityStats>();

        // Snapshottable buffs
        protected readonly List<Buff<CapacityModifier>> capacityBuffs = new List<Buff<CapacityModifier>>();
        protected readonly List<Buff<FirstPassModifier>> firstPassBuffs = new List<Buff<FirstPassModifier>>();
        protected readonly List<Buff<SecondPassModifier>> secondPassBuffs = new List<Buff<SecondPassModifier>>();

        // Unsnapshottable buffs
        protected readonly List<Buff<EnemyBasedModifier>> enemyBasedBuffs = new List<Buff<EnemyBasedModifier>>();
        protected readonly Dictionary<Types, List<Buff<AbilityModifier>>> abilityBuffs = new Dictionary<Types, List<Buff<AbilityModifier>>>();

        // Hooks
        public event EventHandler<Timestamp> skillActivatedHook;
        public event EventHandler<Timestamp> burstActivatedHook;
        public event EventHandler<(Timestamp timestamp, int reaction)> triggeredReactionHook; // TODO Not fired by anything
        public event EventHandler<(Timestamp timestamp, Element? element)> particleCollectedHook; // TODO Not fired by anything

        protected Unit(
            int constellationLevel, Element element, WeaponType weaponType, int burstEnergyCost,
            CapacityStats capacityStats, GeneralStats generalStats, AbilityStats burst, AbilityStats skill, 
            AbilityStats normal, AbilityStats charged, AbilityStats plunge 
        ) {
            this.constellationLevel = constellationLevel;
            this.element = element;
            this.weaponType = weaponType;

            this.burstEnergyCost = burstEnergyCost;
            this.CurrentEnergy = burstEnergyCost;

            this.startingCapacityStats = capacityStats;
            this.startingGeneralStats = generalStats;

            this.startingAbilityStats[Types.NORMAL] = normal;
            this.startingAbilityStats[Types.BURST] = burst;
            this.startingAbilityStats[Types.CHARGED] = charged;
            this.startingAbilityStats[Types.PLUNGE] = plunge;
            this.startingAbilityStats[Types.SKILL] = skill;
        }

        public Weapon Weapon { get; set; }

        public double CurrentHp { get; }
        public double CurrentEnergy { get; private set; }
        public bool IsShielded => throw new NotImplementedException();

        public double GetAbilityGauge(Types type)
        {
            return startingAbilityStats[type].GaugeStrength;
        }

        public List<WorldEvent> SwitchUnit(Timestamp timestamp)
        {
            return new List<WorldEvent> { new SwitchUnit(timestamp, this) };
        }

        public CapacityStats CapacityStats => capacityBuffs.Aggregate(startingCapacityStats, (total, buff) => total + buff.GetModifier());

        public StatsPage GetFirstPassStats(Timestamp timestamp)
        {
            var capacityStats = CapacityStats;
            firstPassBuffs.RemoveAll((buff) => buff.ShouldRemove(timestamp));

            return firstPassBuffs.Aggregate(new StatsPage(capacityStats, startingGeneralStats), (statsPage, buff) => statsPage + buff.GetModifier((this, timestamp, capacityStats)));
        }

        public SecondPassStatsPage GetStatsPage(Timestamp timestamp)
        {
            var stats = new SecondPassStatsPage(GetFirstPassStats(timestamp));
            secondPassBuffs.RemoveAll((buff) => buff.ShouldRemove(timestamp));

            return secondPassBuffs.Aggregate(stats, (statsPage, buff) => statsPage + buff.GetModifier((this, timestamp, stats.firstPassStats)));
        }

        public AbilityStats GetAbilityStats(SecondPassStatsPage statsFromUnit, Types type, Enemy.Enemy enemy, Timestamp timestamp)
        {
            enemyBasedBuffs.RemoveAll((buff) => buff.ShouldRemove(timestamp));
            foreach (var list in abilityBuffs.Values) list.RemoveAll((buff) => buff.ShouldRemove(timestamp));

            AbilityStats result = statsFromUnit.generalStats;

            if (startingAbilityStats.TryGetValue(type, out var startingStats)) result += startingStats;

            if (enemy != null)
            {
                foreach (var buff in enemyBasedBuffs) result += buff.GetModifier((this, timestamp, enemy, statsFromUnit.firstPassStats));
            }

            if (abilityBuffs.TryGetValue(type, out var abilityBuffList))
            {
                foreach (var buff in abilityBuffList) result += buff.GetModifier((this, timestamp, statsFromUnit.firstPassStats));
            }

            return result;
        }
        
        

        public void AddBuff(Buff<CapacityModifier> buff) => buff.AddToList(capacityBuffs);
        public void AddBuff(Buff<FirstPassModifier> buff) => buff.AddToList(firstPassBuffs);
        public void AddBuff(Buff<SecondPassModifier> buff) => buff.AddToList(secondPassBuffs);
        public void AddBuff(Buff<EnemyBasedModifier> buff) => buff.AddToList(enemyBasedBuffs);

        public void AddBuff(Buff<AbilityModifier> buff, params Types[] abilityTypes)
        {
            foreach (var abilityType in abilityTypes)
            {
                if (abilityBuffs.TryGetValue(abilityType, out var list))
                {
                    buff.AddToList(list);
                }
                else
                {
                    list = new List<Buff<AbilityModifier>>();
                    buff.AddToList(list);
                    abilityBuffs.Add(abilityType, list);
                }
            }
        }

        public int GetBuffCount(Guid id)
        {
            return capacityBuffs.Count((buff) => buff.id == id)
                + firstPassBuffs.Count((buff) => buff.id == id)
                + secondPassBuffs.Count((buff) => buff.id == id)
                + enemyBasedBuffs.Count((buff) => buff.id == id)
                + abilityBuffs.Values.SelectMany((list) => list).Distinct().Count((buff) => buff.id == id);
        }

        public void RemoveAllBuffs(Guid id)
        {
            capacityBuffs.RemoveAll((buff) => buff.id == id);
            firstPassBuffs.RemoveAll((buff) => buff.id == id);
            secondPassBuffs.RemoveAll((buff) => buff.id == id);
            enemyBasedBuffs.RemoveAll((buff) => buff.id == id);

            foreach (var list in abilityBuffs.Values) list.RemoveAll((buff) => buff.id == id);
        }

        public void GiveEnergy(int energy) => CurrentEnergy = Math.Min(CurrentEnergy + energy, CapacityStats.Energy);
        public void LoseEnergy(int energy) => CurrentEnergy = Math.Max(CurrentEnergy - energy, 0);

        protected WorldEvent SkillActivated(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (world) => skillActivatedHook?.Invoke(this, timestamp));
        }

        protected WorldEvent BurstActivated(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (world) => burstActivatedHook?.Invoke(this, timestamp));
        }

        protected WorldEvent TriggeredReaction(Timestamp timestamp, int reaction)
        {
            return new WorldEvent(timestamp, (world) => triggeredReactionHook?.Invoke(this, (timestamp, reaction)));
        }

        protected WorldEvent ParticleCollected(Timestamp timestamp, Element? element)
        {
            return new WorldEvent(timestamp, (world) => particleCollectedHook?.Invoke(this, (timestamp, element)));
        }

        public abstract Dictionary<string, Func<Timestamp, List<WorldEvent>>> GetCharacterEvents();
    }
}