namespace Tcc.stats
{
    public class MotionValue
    {
        public double[] MV { get; }
        public double[] Duration { get; }
        public int[] GaugeStrength { get; }

        
        
        public MotionValue(double[] mv, double[] duration, int[] gaugeStrength)
        {
            MV = mv;
            Duration = duration;
            GaugeStrength = gaugeStrength;
        }

    }
}