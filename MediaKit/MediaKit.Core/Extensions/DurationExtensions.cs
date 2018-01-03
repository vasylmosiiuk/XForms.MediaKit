using System;

namespace MediaKit.Core.Extensions
{
    public static class DurationExtensions
    {
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