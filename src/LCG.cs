using System;

namespace Tcc
{
    /// <summary>
    /// Implements a linear congruential generator for fast random number generation
    /// generates numbers by x_{n+1}=a*x_n+c mod m
    /// </summary>
    public class LCG: Random
    {
        private long seed;
        private long mod;
        private double modD;
        private long increment;
        private long multiplier;
        

        private LCG(long a, long c, long m, long s=-1)
        {
            mod = m;
            modD =  m;
            increment = c;
            multiplier = a;
            if (s == -1)
            {
                Random r = new Random();
                seed = r.Next();
            }
            else
            {
                seed = s;
            }
        }
        
        public LCG(): this(11, 25214903917, (long) Math.Pow(2, 48)) {}



        // probably good enough
        public override double NextDouble()
        {
            seed = (multiplier * seed + increment) % mod;
            return  (seed >> 16) / (double) (mod >> 16);
        }
        
    }
}