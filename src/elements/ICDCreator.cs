using System;

namespace Tcc.elements
{
    public class ICDCreator
    {
        // TODO: i think its probably cleaner to add an ICD override somewhere in gauge but i will do that later
        public readonly double TimePity;
        public readonly int HitPity;
        public readonly Guid Guid;

        public ICDCreator(int time, int hit)
        {
            TimePity = (time);
            HitPity = hit;
            Guid = Guid.NewGuid();
        }
        
        public ICDCreator(double time, int hit)
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
            TimePity = (2.5);
            HitPity = 3;
            Guid = Guid.NewGuid();
        }

        public ICD CreateICD()
        {
            return new ICD(TimePity, HitPity);
        }
    }
}