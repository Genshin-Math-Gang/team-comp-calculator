using System;

namespace Tcc.elements
{
    public class ICDCreator
    {
        public readonly Timestamp TimePity;
        public readonly int HitPity;
        public readonly Guid Guid;

        public ICDCreator(int time, int hit, String guid)
        {
            TimePity = new Timestamp(time);
            HitPity = hit;
            Guid = new Guid(guid);
        }
        
        public ICDCreator(Timestamp time, int hit, String guid)
        {
            TimePity = time;
            HitPity = hit;
            Guid = new Guid(guid);
        }
        
        public ICDCreator(String guid)
        {
            TimePity = new Timestamp(2.5);
            HitPity = 3;
            Guid = new Guid(guid);
        }

        public ICDCreator()
        {
            TimePity = new Timestamp(0);
            HitPity = 0;
            Guid = new Guid("00000000-0000-0000-0000-00000000");
        }

        public ICD CreateICD()
        {
            return new ICD(TimePity, HitPity);
        }
    }
}