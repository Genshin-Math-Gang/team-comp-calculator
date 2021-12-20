using System;

namespace Tcc.elements
{
    public class ICDCreator
    {
        // TODO: i think its probably cleaner to add an ICD override somewhere in gauge but i will do that later
        public readonly Timestamp TimePity;
        public readonly int HitPity;
        public readonly Guid Guid;

        public ICDCreator(int time, int hit)
        {
            TimePity = new Timestamp(time);
            HitPity = hit;
            Guid = Guid.NewGuid();
        }
        
        public ICDCreator(Timestamp time, int hit)
        {
            TimePity = time;
            HitPity = hit;
            Guid = Guid.NewGuid();
        }
        
        
        public static ICDCreator Automatic()
        {
            return new ICDCreator(0,0);
        }

        public ICDCreator()
        {
            TimePity = new Timestamp(2.5);
            HitPity = 3;
            Guid = Guid.NewGuid();
        }

        public ICD CreateICD()
        {
            return new ICD(TimePity, HitPity);
        }
    }
}