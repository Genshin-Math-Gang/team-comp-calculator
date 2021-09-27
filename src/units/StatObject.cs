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
    public abstract class StatObject
    {
        // Base stats
        protected readonly CapacityStats startingCapacityStats;
        protected readonly GeneralStats startingGeneralStats;

        // Snapshottable buffs
        protected readonly List<Buff<CapacityModifier>> capacityBuffs = new();
        protected readonly List<Buff<FirstPassModifier>> firstPassBuffs = new();
        protected readonly List<Buff<SecondPassModifier>> secondPassBuffs = new();

        // Unsnapshottable buffs
        protected readonly List<Buff<EnemyBasedModifier>> enemyBasedBuffs = new();
        protected readonly Dictionary<Element, List<Buff<ElementBasedModifier>>> elementBasedBuffs = new();
        protected readonly Dictionary<Types, List<Buff<AbilityModifier>>> abilityBuffs = new();
        
        protected StatObject(GeneralStats stats)
        {
            startingGeneralStats = stats ?? new GeneralStats();
        }
        
        public double CurrentHp { get; }
        public double CurrentEnergy { get; protected set; }
        public bool IsShielded => throw new NotImplementedException();
        
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
        
    }
}