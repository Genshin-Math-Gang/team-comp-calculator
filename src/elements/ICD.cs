using Tcc.Stats;
using Tcc.Units;

namespace Tcc.Elements
{
    class ICD 
    {
        private Timestamp lastHit;
        private int hitPity;

        public ICD(Timestamp lastHit = null, int hitPity = 0)
        {
            this.lastHit = lastHit ?? new Timestamp(0);
            this.hitPity = hitPity;
        }

        public bool checkICD(Timestamp timestmap, int ignoreICD = 0)
        {
            if (ignoreICD == 1) return true;
            if (ignoreICD == -1) return false;

            hitPity += 1;

            if (timestmap - lastHit > new Timestamp(2.5))
            {
                lastHit = timestmap;
                hitPity = 0;
                return true;
            }
            if (hitPity == 3)
            {
                hitPity = 0;
                return true;
            }
            return false;
        }
    }
}