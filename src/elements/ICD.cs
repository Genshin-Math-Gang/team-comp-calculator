namespace Tcc.elements
{
    
    public class ICD 
    {
        private double lastHit;
        private int hitCounter;
        private double timePity;
        private int hitPity;
        
        

        public ICD(double timePity, int hitPity)
        {
            this.timePity = timePity;
            this.hitPity = hitPity;
            this.hitCounter = hitPity - 1;
            this.lastHit = (0);
        }

        public ICD(): this((2.5), 3) {}
        
        public ICD((double, int) param): this(param.Item1, param.Item2) {}
        


        public bool checkICD(double timestamp, int ignoreICD = 0)
        {
            hitCounter += 1;

            if (timestamp - lastHit >= timePity)
            {
                lastHit = timestamp;
                hitCounter = 0;
                return true;
            }
            if (hitCounter == hitPity)
            {
                hitCounter = 0;
                return true;
            }
            return false;
        }
    }
}