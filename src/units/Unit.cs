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
    public abstract class Unit: StatObject
    {
        protected readonly int constellationLevel;
        protected readonly int burstEnergyCost;
        public readonly WeaponType weaponType;
        public readonly Element element;

        // Base stats
        protected readonly CapacityStats startingCapacityStats;
        protected readonly GeneralStats startingGeneralStats;
        protected readonly Dictionary<Types, AbilityStats> startingAbilityStats = new();

        // Snapshottable buffs
        protected readonly List<Buff<CapacityModifier>> capacityBuffs = new();
        protected readonly List<Buff<FirstPassModifier>> firstPassBuffs = new();
        protected readonly List<Buff<SecondPassModifier>> secondPassBuffs = new();

        // Unsnapshottable buffs
        protected readonly List<Buff<EnemyBasedModifier>> enemyBasedBuffs = new();
        protected readonly Dictionary<Element, List<Buff<ElementBasedModifier>>> elementBasedBuffs = new();
        protected readonly Dictionary<Types, List<Buff<AbilityModifier>>> abilityBuffs = new();

        // Hooks
        public event EventHandler<Timestamp> skillActivatedHook;
        public event EventHandler<Timestamp> burstActivatedHook;
        public event EventHandler<(Timestamp timestamp, int reaction)> triggeredReactionHook; // TODO Not fired by anything
        public event EventHandler<(Timestamp timestamp, Element? element)> particleCollectedHook; // TODO Not fired by anything

        public event EventHandler<(Timestamp timestamp, Reaction reaction, Enemy.Enemy enemy)> swirlTriggeredHook;
        

        protected Unit(
            int constellationLevel, Element element, WeaponType weaponType, int burstEnergyCost,
            CapacityStats capacityStats, GeneralStats generalStats, AbilityStats burst, AbilityStats skill, 
            AbilityStats normal, AbilityStats charged, AbilityStats plunge 
        ): base(generalStats) {
            this.constellationLevel = constellationLevel;
            this.element = element;
            this.weaponType = weaponType;

            this.burstEnergyCost = burstEnergyCost;
            this.CurrentEnergy = burstEnergyCost;

            this.startingCapacityStats = capacityStats;

            this.startingAbilityStats[Types.NORMAL] = normal;
            this.startingAbilityStats[Types.BURST] = burst;
            this.startingAbilityStats[Types.CHARGED] = charged;
            this.startingAbilityStats[Types.PLUNGE] = plunge;
            this.startingAbilityStats[Types.SKILL] = skill;
        }

        public Weapon Weapon { get; set; }
        

        public double GetAbilityGauge(Types type)
        {
            // this will probably need to be modified later to make it work with swirl 
            if (type == Types.TRANSFORMATIVE)
            {
                return 0;
            }
            return startingAbilityStats[type].GaugeStrength;
        }

        public List<WorldEvent> SwitchUnit(Timestamp timestamp) 
        {
            return new List<WorldEvent> { new SwitchUnit(timestamp, this) };
        }
        
        public AbilityStats GetAbilityStats(SecondPassStatsPage statsFromUnit, Types type, Element element, Enemy.Enemy enemy, Timestamp timestamp)
        {
            enemyBasedBuffs.RemoveAll((buff) => buff.ShouldRemove(timestamp));
            foreach (var list in abilityBuffs.Values) list.RemoveAll((buff) => buff.ShouldRemove(timestamp));

            AbilityStats result = statsFromUnit.generalStats;

            if (startingAbilityStats.TryGetValue(type, out var startingStats)) result += startingStats;

            if (enemy != null)
            {
                foreach (var buff in enemyBasedBuffs) result += buff.GetModifier((this, timestamp, enemy, statsFromUnit.firstPassStats));
            }

            if (elementBasedBuffs.TryGetValue(element, out var elementBuffList))
            {
                foreach (var buff in elementBuffList) result += buff.GetModifier((this, timestamp, statsFromUnit.firstPassStats));
            }

            if (abilityBuffs.TryGetValue(type, out var abilityBuffList))
            {
                foreach (var buff in abilityBuffList) result += buff.GetModifier((this, timestamp, statsFromUnit.firstPassStats));
            }

            return result;
        }
        

        public void GiveEnergy(int energy) => CurrentEnergy = Math.Min(CurrentEnergy + energy, CapacityStats.Energy);
        public void LoseEnergy(int energy) => CurrentEnergy = Math.Max(CurrentEnergy - energy, 0);

        protected WorldEvent SkillActivated(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (world) => skillActivatedHook?.Invoke(this, timestamp),$"Skill activated by {this}");
        }

        protected WorldEvent BurstActivated(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (world) => burstActivatedHook?.Invoke(this, timestamp), $"Burst activated by {this}");
        }

        protected WorldEvent TriggeredReaction(Timestamp timestamp, int reaction)
        {
            return new WorldEvent(timestamp, (world) => triggeredReactionHook?.Invoke(this, (timestamp, reaction)));
        }

        protected WorldEvent ParticleCollected(Timestamp timestamp, Element? element)
        {
            return new WorldEvent(timestamp, (world) => particleCollectedHook?.Invoke(this, (timestamp, element)));
        }

        public WorldEvent TriggeredSwirl(Timestamp timestamp, Reaction reaction, Enemy.Enemy enemy)
        {
            if (!ReactionTypes.IsSwirl(reaction))
            {
                // throw some error
            }

            return new WorldEvent(timestamp, _ => swirlTriggeredHook?.Invoke(this, (timestamp, reaction, enemy)),
                $"{this} triggered swirl at {timestamp}", 1);
        }

        //public abstract Dictionary<string, Delegate> GetCharacterEvents();
    }
}