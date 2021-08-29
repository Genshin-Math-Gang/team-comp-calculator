using System;
namespace Tcc
{
    // We may eventually want to use frames, this abstraction means we change it in one place
    public sealed class Timestamp: IComparable<Timestamp>
    {
        readonly double time;

        public Timestamp(double time)
        {
            this.time = time;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return time == ((Timestamp)obj).time;
        }

        public override int GetHashCode()
        {
            return time.GetHashCode();
        }

        // Would get reduced from double to int and replaced in favour of a function to generate a timestamp from frames if we switch
        public static Timestamp operator +(Timestamp time, double seconds) => time + new Timestamp(seconds);
        public static Timestamp operator +(double seconds, Timestamp time) => time + new Timestamp(seconds);

        public static Timestamp operator -(Timestamp time, double seconds) => time - new Timestamp(seconds);
        public static Timestamp operator -(double seconds, Timestamp time) => new Timestamp(seconds) - time;

        public static Timestamp operator -(Timestamp first, Timestamp second) => new Timestamp(first.time - second.time);
        public static Timestamp operator +(Timestamp time1, Timestamp time2) => time1 == null || time2 == null ? new Timestamp(0) : new Timestamp(time1.time + time2.time);
        public static Timestamp operator *(int scalar, Timestamp time) => new Timestamp(scalar * time.time);
        public static Timestamp operator *(double scalar, Timestamp time) => new Timestamp(scalar * time.time);
        public static Timestamp operator * (Timestamp scalar, Timestamp time) => new Timestamp(scalar.time * time.time);

        public static bool operator ==(Timestamp time1, Timestamp time2) => time1?.time == time2?.time;
        public static bool operator !=(Timestamp time1, Timestamp time2) => time1?.time != time2?.time;
        public static bool operator >(Timestamp time1, Timestamp time2) => time1?.time > time2?.time;
        public static bool operator >=(Timestamp time1, Timestamp time2) => time1?.time >= time2?.time;
        public static bool operator <(Timestamp time1, Timestamp time2) => time1?.time < time2?.time;
        public static bool operator <=(Timestamp time1, Timestamp time2) => time1?.time <= time2?.time;

        public static implicit operator double(Timestamp time) => time.time;

        public int CompareTo(Timestamp other)
        {
            if(other == null) throw new ArgumentNullException(nameof(other));

            return this.time.CompareTo(other.time);
        }

        public override string ToString() => $"t = {time}";
    }
}