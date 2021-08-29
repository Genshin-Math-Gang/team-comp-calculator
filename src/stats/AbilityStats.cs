using System;
using Tcc.Elements;

namespace Tcc.Stats
{
    public class AbilityStats
    {
        readonly GeneralStats generalStats;
        readonly double[] motionValues;

        public AbilityStats(GeneralStats generalStats = null, double[] motionValues = null, Timestamp icd = null)
        {
            this.generalStats = generalStats ?? new GeneralStats();
            this.motionValues = motionValues;
            this.Icd = icd ?? new Timestamp(0);
        }

        public Timestamp Icd { get; }

        public double CalculateHitDamage(int mvIndex, Element element)
        {
            var stats = generalStats.NonKeyedStats + generalStats.ElementalBonus.GetStatBonus(element);

            return motionValues[mvIndex] * stats.Attack * stats.DamagePercent * (1 + stats.CritRate * stats.CritDamage) * stats.IndependentMultiplier;
        }

        public double GetMotionValue(int mvIndex) => motionValues[mvIndex];

        public static AbilityStats operator +(AbilityStats first, AbilityStats second)
        {
            double[] motionValues = null;

            if (first.motionValues != null)
            {
                if (second.motionValues != null)
                {
                    if (first.motionValues.Length != second.motionValues.Length) throw new Exception("Cannot add different motion values together");

                    motionValues = new double[first.motionValues.Length];

                    for (int i = 0; i < motionValues.Length; i++)
                    {
                        motionValues[i] = first.motionValues[i] + second.motionValues[i];
                    }
                }
                else
                {
                    motionValues = (double[])first.motionValues.Clone();
                }
            }
            else if (second.motionValues != null)
            {
                motionValues = (double[])second.motionValues.Clone();
            }

            return new AbilityStats(
                generalStats: first.generalStats + second.generalStats,
                motionValues: motionValues
            );
        }

        public static implicit operator AbilityStats(GeneralStats generalStats) => new AbilityStats(generalStats: generalStats);
    }
}