using System;
using Tcc.Elements;

namespace Tcc.Stats
{
    public class AbilityStats
    {
        readonly StatsPage statsPage;
        readonly double[] motionValues;


        public AbilityStats(StatsPage statsPage = null, double[] motionValues = null)
        {
            this.statsPage = statsPage ?? new StatsPage();
            this.motionValues = motionValues;
        }


        
        public double CalculateHitMultiplier(int index, Element element)
        {
            // what the fuck is independent multiplier
            return statsPage.Atk * statsPage.CritMultiplier * statsPage.DamageMultiplier(element) 
                   * motionValues[index] * (1 + statsPage[Stats.IndependentMultiplier]);
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
                statsPage: first.statsPage + second.statsPage,
                motionValues: motionValues
            );
        }

        
        public static implicit operator AbilityStats(SecondPassStatsPage statsPage) => new (statsPage.firstPassStats);
        public static implicit operator AbilityStats(StatsPage statsPage) => new (statsPage);
        
        public static implicit operator AbilityStats((Stats, double) data) => new StatsPage(data.Item1, data.Item2);
    }
}