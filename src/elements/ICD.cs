namespace Tcc.elements
{
    
    public class ICD 
    {
        private Timestamp lastHit;
        private int hitCounter;
        private Timestamp timePity;
        private int hitPity;
        
        

        public ICD(Timestamp timePity, int hitPity)
        {
            this.timePity = timePity;
            this.hitPity = hitPity;
            this.hitCounter = hitPity - 1;
            this.lastHit = new Timestamp(0);
        }

        public ICD(): this(new Timestamp(2.5), 3) {}
        
        public ICD((Timestamp, int) param): this(param.Item1, param.Item2) {}
        


        public bool checkICD(Timestamp timestamp, int ignoreICD = 0)
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