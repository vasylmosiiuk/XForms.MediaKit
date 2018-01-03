using System;

namespace MediaKit.Core.Extensions
{
    public static class TimeSpanExtensions
    {
        //https://stackoverflow.com/a/45205696
        public static TimeSpan SumSafety(this TimeSpan ts1, TimeSpan ts2)
        {
            bool sign1 = ts1 < TimeSpan.Zero, sign2 = ts2 < TimeSpan.Zero;

            if (sign1 && sign2)
            {
                if (TimeSpan.MinValue - ts1 > ts2)
                    return TimeSpan.MinValue;
            }
            else if (!sign1 && !sign2)
            {
                if (TimeSpan.MaxValue - ts1 < ts2)
                    return TimeSpan.MaxValue;
            }

            return ts1 + ts2;
        }

        public static TimeSpan MulSafety(this TimeSpan ts1, uint times)
        {
            var sign1 = ts1 < TimeSpan.Zero;

            if (sign1)
                return TimeSpan.Zero;
            if (TimeSpan.MaxValue.TotalMilliseconds - ts1.TotalMilliseconds * times < 0)
                return TimeSpan.MaxValue;

            return TimeSpan.FromMilliseconds(ts1.TotalMilliseconds * times);
        }
    }
}