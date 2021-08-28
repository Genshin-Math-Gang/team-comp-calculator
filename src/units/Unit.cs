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
        protected readonly Element element;

        // Base stats
        protected readonly CapacityStats startingCapacityStats;
        protected readonly GeneralStats startingGeneralStats;
        protected readonly Dictionary<Types, AbilityStats> startingAbilityStats;

        // Buffs
        protected readonly List<Buff<CapacityModifier>> capacityBuffs = new List<Buff<CapacityModifier>>();
        protected readonly List<Buff<FirstPassModifier>> firstPassBuffs = new List<Buff<FirstPassModifier>>();
        protected readonly List<Buff<SecondPassModifier>> secondPassBuffs = new List<Buff<SecondPassModifier>>();
        protected readonly List<Buff<EnemyBasedModifier>> enemyBasedBuffs = new List<Buff<EnemyBasedModifier>>();

        protected readonly Dictionary<Element, List<Buff<ElementModifier>>> elementBuffs = new Dictionary<Element, List<Buff<ElementModifier>>>();
        protected readonly Dictionary<Types, List<Buff<AbilityModifier>>> abilityBuffs = new Dictionary<Types, List<Buff<AbilityModifier>>>();
        protected readonly Dictionary<Reaction, List<Buff<ReactionPercentModifier>>> reactionPercentBuffs = new Dictionary<Reaction, List<Buff<ReactionPercentModifier>>>();

        // Hooks
        public event EventHandler<Timestamp> skillActivatedHook;
        public event EventHandler<Timestamp> burstActivatedHook;
        public event EventHandler<(Timestamp timestamp, Reaction reaction)> triggeredReactionHook; // TODO Not fired by anything
        public event EventHandler<(Timestamp timestamp, Element? element)> particleCollectedHook; // TODO Not fired by anything

        protected Unit(
            int constellationLevel, Element element, int burstEnergyCost,
            CapacityStats capacityStats, GeneralStats generalStats,
            AbilityStats burst, AbilityStats skill, AbilityStats normal, AbilityStats charged, AbilityStats plunge
        ) {
            this.constellationLevel = constellationLevel;
            this.element = element;

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

        public List<WorldEvent> SwitchUnit(Timestamp timestamp)
        {
            return new List<WorldEvent> { new SwitchUnit(timestamp, this) };
        }

        public CapacityStats CapacityStats => capacityBuffs.Aggregate(startingCapacityStats, (total, buff) => total + buff.GetModifier());

        protected StatsPage GetFirstPassStats(Timestamp timestamp)
        {
            var capacityStats = CapacityStats;
            firstPassBuffs.RemoveAll((buff) => buff.ShouldRemove(timestamp));

            return firstPassBuffs.Aggregate(new StatsPage(capacityStats), (statsPage, buff) => statsPage + buff.GetModifier((this, timestamp, capacityStats)));
        }

        protected StatsPage GetSecondPassStats(Timestamp timestamp)
        {
            var firstPassStats = GetFirstPassStats(timestamp);
            secondPassBuffs.RemoveAll((buff) => buff.ShouldRemove(timestamp));

            return secondPassBuffs.Aggregate(firstPassStats, (statsPage, buff) => statsPage + buff.GetModifier((this, timestamp, firstPassStats)));
        }

        //public Stats.Stats GetStats(Types type, Enemy.Enemy enemy, Timestamp timestamp)
        //{
        //    var result = GetStatsFromUnit(type, timestamp);
        //    result = AddStatsFromEnemy(result, type, enemy, timestamp);

        //    return result;
        //}

        //public Stats.Stats GetStatsFromUnitWithoutScaled(Types type, Timestamp timestamp)
        //{
        //    buffsFromUnit.RemoveAll((buff) => buff.HasExpired(timestamp));

        //    var firstPassStats = modifiers[type] + stats;
        //    foreach(var buff in buffsFromUnit) firstPassStats += buff.GetModifier(this, type);

        //    return firstPassStats;
        //}

        //public Stats.Stats GetStatsFromUnit(Types type, Timestamp timestamp)
        //{
        //    buffsFromUnit.RemoveAll((buff) => buff.HasExpired(timestamp));
        //    buffsFromStats.RemoveAll((buff) => buff.HasExpired(timestamp));

        //    var firstPassStats = modifiers[type] + stats;
        //    foreach(var buff in buffsFromUnit) firstPassStats += buff.GetModifier(this, type);

        //    var result = firstPassStats;
        //    foreach(var buff in buffsFromStats) result += buff.GetModifier(this, firstPassStats, timestamp, type);

        //    return result;
        //}

        //public Stats.Stats SnapshotStats(Timestamp timestamp)
        //{
        //    buffsFromUnit.RemoveAll((buff) => buff.HasExpired(timestamp));
        //    buffsFromStats.RemoveAll((buff) => buff.HasExpired(timestamp));

        //    var firstPassStats = stats;
        //    foreach(var buff in buffsFromUnit) 
        //    {
        //        firstPassStats += buff.GetModifier(this, Types.GENERIC);
        //    }

        //    var result = firstPassStats;
        //    foreach(var buff in buffsFromStats) result += buff.GetModifier(this, firstPassStats, timestamp, Types.GENERIC);

        //    return result;
        //}

        //public Stats.Stats AddStatsFromEnemy(Stats.Stats statsFromUnit, Types type, Enemy.Enemy enemy, Timestamp timestamp)
        //{
        //    buffsFromEnemy.RemoveAll((buff) => buff.HasExpired(timestamp));

        //    var result = statsFromUnit;

        //    if(enemy != null)
        //    {
        //        foreach(var buff in buffsFromEnemy) result += buff.GetModifier(enemy, type);
        //    }

        //    return result;
        //}

        //protected Func<Enemy.Enemy, Timestamp, Stats.Stats> GetStats(Types type)
        //{
        //    return (enemy, timestamp) => GetStats(type, enemy, timestamp);
        //}

        public void AddBuff(Buff<CapacityModifier> buff) => buff.AddToList(capacityBuffs);
        public void AddBuff(Buff<FirstPassModifier> buff) => buff.AddToList(firstPassBuffs);
        public void AddBuff(Buff<SecondPassModifier> buff) => buff.AddToList(secondPassBuffs);
        public void AddBuff(Buff<EnemyBasedModifier> buff) => buff.AddToList(enemyBasedBuffs);

        public void AddBuff(Buff<ElementModifier> buff, params Element[] elements)
        {
            foreach (var element in elements)
            {
                if (elementBuffs.TryGetValue(element, out var list))
                {
                    buff.AddToList(list);
                }
                else
                {
                    list = new List<Buff<ElementModifier>>();
                    buff.AddToList(list);
                    elementBuffs.Add(element, list);
                }
            }
        }

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

        public void AddBuff(Buff<ReactionPercentModifier> buff, params Reaction[] reactions)
        {
            foreach (var reaction in reactions)
            {
                if (reactionPercentBuffs.TryGetValue(reaction, out var list))
                {
                    buff.AddToList(list);
                }
                else
                {
                    list = new List<Buff<ReactionPercentModifier>>();
                    buff.AddToList(list);
                    reactionPercentBuffs.Add(reaction, list);
                }
            }
        }

        public int GetBuffCount(Guid id)
        {
            return capacityBuffs.Count((buff) => buff.id == id)
                + firstPassBuffs.Count((buff) => buff.id == id)
                + secondPassBuffs.Count((buff) => buff.id == id)
                + enemyBasedBuffs.Count((buff) => buff.id == id)
                + elementBuffs.Values.SelectMany((list) => list).Distinct().Count((buff) => buff.id == id)
                + abilityBuffs.Values.SelectMany((list) => list).Distinct().Count((buff) => buff.id == id)
                + reactionPercentBuffs.Values.SelectMany((list) => list).Distinct().Count((buff) => buff.id == id);
        }

        public void RemoveAllBuffs(Guid id)
        {
            capacityBuffs.RemoveAll((buff) => buff.id == id);
            firstPassBuffs.RemoveAll((buff) => buff.id == id);
            secondPassBuffs.RemoveAll((buff) => buff.id == id);
            enemyBasedBuffs.RemoveAll((buff) => buff.id == id);

            foreach (var list in elementBuffs.Values) list.RemoveAll((buff) => buff.id == id);
            foreach (var list in abilityBuffs.Values) list.RemoveAll((buff) => buff.id == id);
            foreach (var list in reactionPercentBuffs.Values) list.RemoveAll((buff) => buff.id == id);
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