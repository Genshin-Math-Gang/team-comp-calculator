using System.Collections.Generic;
using System.Linq;

namespace Tcc.Stats
{
    public class KeyedStatBonus<KeyT>
    {
        readonly Dictionary<KeyT, NonKeyedStats> bonuses;

        KeyedStatBonus(Dictionary<KeyT, NonKeyedStats> bonuses)
        {
            this.bonuses = bonuses;
        }

        public KeyedStatBonus(params (KeyT key, NonKeyedStats stats)[] bonuses)
        {
            this.bonuses = bonuses.ToDictionary((bonus) => bonus.key, (bonus) => bonus.stats);
        }

        public KeyedStatBonus(KeyT key, NonKeyedStats stats) : this((key, stats))
        {
        }

        public NonKeyedStats GetStatBonus(KeyT key)
        {
            return bonuses.TryGetValue(key, out var stats) ? stats : new NonKeyedStats();
        }

        public static KeyedStatBonus<KeyT> operator +(KeyedStatBonus<KeyT> first, KeyedStatBonus<KeyT> second)
        {
            var result = new Dictionary<KeyT, NonKeyedStats>(first.bonuses);

            foreach(var (key, stats) in second.bonuses)
            {
                if(result.ContainsKey(key)) result[key] += stats;
                else result[key] = stats;
            }

            return new KeyedStatBonus<KeyT>(result);
        }
    }
}