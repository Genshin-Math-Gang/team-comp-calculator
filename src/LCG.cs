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
        private long increment;
        private long multiplier;
        

        private LCG(long a, long c, long m, long s=-1)
        {
            mod = m;
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
        
        public LCG(): this((long) Math.Pow(2, 48), 25214903917, 11) {}



        public override double NextDouble()
        {
            seed = (multiplier * seed + increment) % mod;
            return (seed >> 16) / (double) mod;
        }
        
    }
}