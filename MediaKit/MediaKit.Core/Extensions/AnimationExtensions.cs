namespace MediaKit.Core.Extensions
{
    public static class AnimationExtensions
    {
        public static Duration CalculateExactAnimationDuration(this IAnimation animation)
        {
            var singleAnimationDuration = animation.Duration;
            var repeatBehavior = animation.RepeatBehavior;
            if (repeatBehavior == RepeatBehavior.Forever)
                return Duration.Forever;
            if (singleAnimationDuration.HasTimeSpan)
            {
                if (repeatBehavior.HasCount)
                    return new Duration(singleAnimationDuration.TimeSpan.MulSafety(repeatBehavior.Count));
                if (repeatBehavior.HasDuration)
                    return new Duration(repeatBehavior.Duration);
            }


            return singleAnimationDuration;
        }
    }
}