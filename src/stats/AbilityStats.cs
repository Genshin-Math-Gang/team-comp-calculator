using System;
using Tcc.elements;

namespace Tcc.stats
{
    public class AbilityStats
    {
         StatsPage statsPage;
         double[] motionValues;


        public AbilityStats(StatsPage statsPage = null, double[] motionValues = null)
        {
            this.statsPage = statsPage ?? new StatsPage();
            this.motionValues = motionValues;
        }


        
        public double CalculateHitMultiplier(Element element, Random r, bool deterministic)
        {
            return statsPage.Atk * statsPage.CritMultiplier(r, deterministic) * statsPage.DamageMultiplier(element) *
                   (1 + statsPage[Stats.IndependentMultiplier]);
        }

        public double GetMotionValue(int mvIndex) => motionValues[mvIndex];

        public void Add(AbilityStats stats)
        {

            if (motionValues != null)
            {
                if (stats.motionValues != null)
                {
                    if (motionValues.Length != stats.motionValues.Length) throw new Exception("Cannot add different motion values together");
                    

                    for (int i = 0; i < motionValues.Length; i++)
                    {
                        motionValues[i] += stats.motionValues[i];
                    }
                }
                else
                {
                    motionValues = (double[])motionValues.Clone();
                }
            }
            else if (stats.motionValues != null)
            {
                motionValues = (double[])stats.motionValues.Clone();
            }

            statsPage.Add(stats.statsPage);
        }

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