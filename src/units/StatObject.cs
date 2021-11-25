using System;
using System.Collections.Generic;
using System.Linq;
using Tcc.buffs;
using Tcc.elements;
using Tcc.stats;

namespace Tcc.units
{
    public abstract class StatObject
    {
        // Base stats
        // i made these non-readonly to make some very sus workaround but this means i need to be super careful
        protected StatsPage StartingStatsPage;
        protected string level;
        public int Level
        {
            get
            {
                return level switch
                {
                    "1" => 1,
                    "20" or "20+" => 20,
                    "40" or "40+" => 40,
                    "50" or "50+" => 50,
                    "60" or "60+" => 60,
                    "70" or "70+" => 70,
                    "80" or "80+" => 80,
                    "90" => 90,
                    _ => throw new ArgumentException($"bad level given {level}")
                };
            }
        }

        private double currentHp;
        public double CurrentHp
        {
            get => currentHp;
            set => currentHp = Math.Max(0, value);
        }
        

        // Snapshottable buffs
        protected readonly List<Buff<CapacityModifier>> CapacityBuffs = new();
        protected readonly List<Buff<FirstPassModifier>> FirstPassBuffs = new();
        protected readonly List<Buff<SecondPassModifier>> SecondPassBuffs = new();

        // Unsnapshottable buffs
        protected readonly List<Buff<EnemyBasedModifier>> EnemyBasedBuffs = new();
        protected readonly Dictionary<Element, List<Buff<ElementBasedModifier>>> ElementBasedBuffs = new();
        protected readonly Dictionary<Types, List<Buff<AbilityModifier>>> AbilityBuffs = new();
        
        protected StatObject(string level = "90")
        {
            this.level = level;
            StartingStatsPage = new StatsPage();
            currentHp = StartingStatsPage.Hp;
        }
        
        protected StatObject(StatsPage statsPage, string level = "90")
        {
            this.level = level;
            StartingStatsPage = statsPage;
            currentHp = StartingStatsPage.Hp;
        }
        
        /*public double CurrentHp {
            get { return CapacityStats.Hp.Current; }
        }*/
        public double CurrentEnergy { get; protected set; }
        public bool IsShielded => throw new NotImplementedException();
        
        //public CapacityStats CapacityStats => CapacityBuffs.Aggregate(StartingCapacityStats, (total, buff) => total + buff.GetModifier());

        public StatsPage GetFirstPassStats(Timestamp timestamp)
        {
            FirstPassBuffs.RemoveAll((buff) => buff.ShouldRemove(timestamp));

            return FirstPassBuffs.Aggregate(StartingStatsPage, 
                (statsPage, buff) => statsPage + buff.GetModifier((this, timestamp)));
        }

        public SecondPassStatsPage GetStatsPage(Timestamp timestamp)
        {
            var stats = new SecondPassStatsPage(GetFirstPassStats(timestamp));
            SecondPassBuffs.RemoveAll((buff) => buff.ShouldRemove(timestamp));

            return SecondPassBuffs.Aggregate(stats, (statsPage, buff) => statsPage + buff.GetModifier((this, timestamp, stats.firstPassStats)));
        }
        
        public void AddBuff(Buff<CapacityModifier> buff) => buff.AddToList(CapacityBuffs);
        public void AddBuff(Buff<FirstPassModifier> buff) => buff.AddToList(FirstPassBuffs);
        public void AddBuff(Buff<SecondPassModifier> buff) => buff.AddToList(SecondPassBuffs);
        public void AddBuff(Buff<EnemyBasedModifier> buff) => buff.AddToList(EnemyBasedBuffs);

        public void AddBuff(Buff<AbilityModifier> buff, params Types[] abilityTypes)
        {
            foreach (var abilityType in abilityTypes)
            {
                if (AbilityBuffs.TryGetValue(abilityType, out var list))
                {
                    buff.AddToList(list);
                }
                else
                {
                    list = new List<Buff<AbilityModifier>>();
                    buff.AddToList(list);
                    AbilityBuffs.Add(abilityType, list);
                }
            }
        }

        public int GetBuffCount(Guid id)
        {
            return CapacityBuffs.Count((buff) => buff.id == id)
                   + FirstPassBuffs.Count((buff) => buff.id == id)
                   + SecondPassBuffs.Count((buff) => buff.id == id)
                   + EnemyBasedBuffs.Count((buff) => buff.id == id)
                   + AbilityBuffs.Values.SelectMany((list) => list).Distinct().Count((buff) => buff.id == id);
        }

        public void RemoveAllBuffs(Guid id)
        {
            CapacityBuffs.RemoveAll((buff) => buff.id == id);
            FirstPassBuffs.RemoveAll((buff) => buff.id == id);
            SecondPassBuffs.RemoveAll((buff) => buff.id == id);
            EnemyBasedBuffs.RemoveAll((buff) => buff.id == id);

            foreach (var list in AbilityBuffs.Values) list.RemoveAll((buff) => buff.id == id);
        }
        
    }
}