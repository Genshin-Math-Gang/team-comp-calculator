namespace Tcc.Stats
{
    public class MotionValue
    {
        public double[] MV { get; }
        public Timestamp[] Duration { get; }
        public int[] GaugeStrength { get; }

        
        
        public MotionValue(double[] mv, Timestamp[] duration, int[] gaugeStrength)
        {
            MV = mv;
            Duration = duration;
            GaugeStrength = gaugeStrength;
        }

    }
}