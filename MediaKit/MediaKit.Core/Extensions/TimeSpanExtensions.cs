using System;

namespace MediaKit.Core.Extensions
{
    /// <summary>
    ///     <see cref="TimeSpan" /> specific extensions, used by MediaKit infrastructure.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        ///     Combine <see cref="TimeSpan" /> values safety.
        /// </summary>
        /// <param name="ts1">Left value</param>
        /// <param name="ts2">Right value</param>
        /// <returns>
        ///     Sum of both <see cref="TimeSpan" /> values. In case the sum is out of possible values,
        ///     <see cref="TimeSpan.MinValue" /> or <see cref="TimeSpan.MaxValue" /> will be returned.
        /// </returns>
        /// <remarks>https://stackoverflow.com/a/45205696</remarks>
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

        /// <summary>
        ///     Multiplies specified value in count of times, you specified.
        /// </summary>
        /// <param name="ts1"><see cref="TimeSpan" />, you want to use as base.</param>
        /// <param name="times">Count of times, you want to multiple <paramref name="ts1" /> value.</param>
        /// <returns>
        ///     <see cref="TimeSpan" /> value in range [<see cref="TimeSpan.Zero" />;<see cref="TimeSpan.MaxValue" />],
        ///     returned by multiplying <see cref="TimeSpan.TotalMilliseconds" /> by <paramref name="times" />.
        /// </returns>
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