using System;

namespace MediaKit.Core.Extensions
{
    /// <summary>
    ///     <see cref="Duration" /> specific extensions, used by MediaKit infrastructure.
    /// </summary>
    public static class DurationExtensions
    {
        /// <summary>
        ///     Converts target <see cref="Duration" /> value into <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="duration">Target <see cref="Duration" /></param>
        /// <returns>
        ///     <see cref="Duration.TimeSpan" /> value of target duration in case it contains TimeSpan (explicit or
        ///     calculated). <see cref="TimeSpan.Zero" /> in case target duration is <see cref="Duration.Automatic" />.
        ///     2 days in case target duration is <see cref="Duration.Forever" />
        /// </returns>
        public static TimeSpan ToTimeSpan(this Duration duration)
        {
            if (duration.HasTimeSpan)
                return duration.TimeSpan;
            if (duration == Duration.Automatic)
                return TimeSpan.Zero;
            if (duration == Duration.Forever)
                return TimeSpan.FromDays(2);

            throw new InvalidOperationException("Unknown duration type");
        }
    }
}