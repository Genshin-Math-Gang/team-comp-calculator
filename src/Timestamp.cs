using System;

namespace Tcc
{
    /*
    // We may eventually want to use frames, this abstraction means we change it in one place
    public sealed class double : IComparable<double>
    {
        readonly double time;

        public double(double time)
        {
            this.time = time;
        }

        public double()
        {
            this.time = 0;
        }

        public int CompareTo(double other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            return this.time.CompareTo(other.time);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return time == ((double) obj).time;
        }

        public override int GetHashCode()
        {
            return time.GetHashCode();
        }

        // Would get reduced from double to int and replaced in favour of a function to generate a timestamp from frames if we switch

        public static double operator -(double first, double second) =>
            (first.time - second.time);

        public static double operator +(double time1, double time2) =>
            time1 == null || time2 == null ? (0) : (time1.time + time2.time);

        public static double operator *(int scalar, double time) => (scalar * time.time);
        public static double operator *(double scalar, double time) => (scalar * time.time);
        public static double operator *(double scalar, double time) => (scalar.time * time.time);

        public static bool operator ==(double time1, double time2) => time1?.time == time2?.time;
        public static bool operator !=(double time1, double time2) => time1?.time != time2?.time;
        public static bool operator >(double time1, double time2) => time1?.time > time2?.time;
        public static bool operator >=(double time1, double time2) => time1?.time >= time2?.time;
        public static bool operator <(double time1, double time2) => time1?.time < time2?.time;
        public static bool operator <=(double time1, double time2) => time1?.time <= time2?.time;

        public static implicit operator double(double time) => time.time;

        public override string ToString() => $"t = {time:F3}";

        public static double Max(double t1, double t2)
        {
            return t1.CompareTo(t2) == 1 ? t1 : t2;
        }

        public static implicit operator double(double d) => new(d);
    }*/
}