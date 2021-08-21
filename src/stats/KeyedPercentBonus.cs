using System.Collections.Generic;
using System.Linq;

namespace Tcc.Stats
{
    public class KeyedPercentBonus<KeyT>
    {
        readonly Dictionary<KeyT, double> damagePercentBonuses;

        KeyedPercentBonus(Dictionary<KeyT, double> damagePercentBonuses)
        {
            this.damagePercentBonuses = damagePercentBonuses;
        }

        public KeyedPercentBonus(params (KeyT key, double damagePercent)[] bonuses)
        {
            this.damagePercentBonuses = bonuses.ToDictionary((bonus) => bonus.key, (bonus) => bonus.damagePercent);
        }

        public KeyedPercentBonus(KeyT key, double damagePercent): this((key, damagePercent))
        {
        }

        public double GetDamagePercentBonus(KeyT key)
        {
            return damagePercentBonuses.TryGetValue(key, out var damagePercent) ? damagePercent : 0;
        }

        public static KeyedPercentBonus<KeyT> operator +(KeyedPercentBonus<KeyT> bonus1, KeyedPercentBonus<KeyT> bonus2)
        {
            var result = new Dictionary<KeyT, double>(bonus1.damagePercentBonuses);

            foreach(var (key, damagePercent) in bonus2.damagePercentBonuses)
            {
                if(result.ContainsKey(key)) result[key] += damagePercent;
                else result[key] = damagePercent;
            }

            return new KeyedPercentBonus<KeyT>(result);
        }

        public static KeyedPercentBonus<KeyT> operator *(double scalar, KeyedPercentBonus<KeyT> bonus)
        {
            var result = new Dictionary<KeyT, double>();

            foreach(var (key, damagePercent) in bonus.damagePercentBonuses)
            {
                result.Add(key, damagePercent * scalar);
            }

            return new KeyedPercentBonus<KeyT>(result);
        }
    }
}