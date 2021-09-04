using System;
using Tcc.Elements;

namespace Tcc.Stats
{
    public class AbilityStats
    {
        readonly GeneralStats generalStats;
        readonly double[] motionValues;
        public Timestamp ICD { get; }
        public double GaugeStrength;

        public AbilityStats(GeneralStats generalStats = null, double[] motionValues = null, Timestamp icd = null, double gaugeStrength = 1)
        {
            this.generalStats = generalStats ?? new GeneralStats();
            this.motionValues = motionValues;
            this.ICD = icd ?? new Timestamp(0);
            this.GaugeStrength = gaugeStrength;
        }

        public double CalculateHitDamage(int mvIndex, Element element)
        {
            var damagePercent = generalStats.DamagePercent + generalStats.ElementalBonus.GetPercentBonus(element);

            return motionValues[mvIndex] * generalStats.Attack * (1 + damagePercent) * (1 + generalStats.CritRate * generalStats.CritDamage) * generalStats.IndependentMultiplier;
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