namespace MediaKit.Core.Extensions
{
    /// <summary>
    ///     <see cref="IAnimation" /> specific extensions, used by MediaKit infrastructure.
    /// </summary>
    public static class AnimationExtensions
    {
        /// <summary>
        ///     Calculates specified animation's duration, due to <see cref="IAnimation.RepeatBehavior" /> and
        ///     <see cref="IAnimation.Duration" /> values.
        /// </summary>
        /// <param name="animation">Target animation.</param>
        /// <returns>
        ///     <see cref="Duration.Forever" /> in case if target animation specified with
        ///     <see cref="IAnimation.RepeatBehavior" /> eq. <see cref="RepeatBehavior.Forever" />
        ///     <see cref="Duration" /> with specific timespan value, calculated, based on <see cref="IAnimation.RepeatBehavior" />
        ///     type (<see cref="RepeatBehavior.HasDuration" /> or <see cref="RepeatBehavior.HasTimes" />).
        ///     In case it defined with times count, resulting duration will be product of safety multiplying
        ///     <see cref="IAnimation.Duration" /> in
        ///     <see cref="RepeatBehavior.Times" />.
        ///     In case it defined with timespan value, resulting duration will be constructed, based on that timespan.
        /// </returns>
        public static Duration CalculateExactAnimationDuration(this IAnimation animation)
        {
            var singleAnimationDuration = animation.Duration;
            var repeatBehavior = animation.RepeatBehavior;
            if (repeatBehavior == RepeatBehavior.Forever)
                return Duration.Forever;
            if (singleAnimationDuration.HasTimeSpan)
            {
                if (repeatBehavior.HasTimes)
                    return new Duration(singleAnimationDuration.TimeSpan.MulSafety(repeatBehavior.Times));
                if (repeatBehavior.HasDuration)
                    return new Duration(repeatBehavior.Duration);
            }


            return singleAnimationDuration;
        }
    }
}