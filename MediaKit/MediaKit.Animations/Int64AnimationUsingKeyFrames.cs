using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    /// <summary>
    ///     KeyFrame animation which proceed <see cref="long" /> values.
    /// </summary>
    public sealed class Int64AnimationUsingKeyFrames : KeyFrameAnimation<long>
    {
    }

    /// <summary>
    ///     Discrete animation key frame which maintain <see cref="long" /> values.
    /// </summary>
    public sealed class DiscreteInt64KeyFrame : DiscreteKeyFrame<long>
    {
    }

    /// <summary>
    ///     Linear animation key frame which maintain <see cref="long" /> values.
    /// </summary>
    public sealed class LinearInt64KeyFrame : LinearKeyFrame<long>
    {
        /// <summary>
        ///     Returns interpolated value between <paramref name="initialValue" /> begin and
        ///     <see cref="KeyFrame{TValue}.Value" />, with <paramref name="x" /> as interpolation position.
        /// </summary>
        /// <param name="x">Interpolation position, value in range [0;1].</param>
        /// <param name="initialValue">Interpolation From value.</param>
        /// <returns></returns>
        protected override long GetInterpolatedValue(double x, long initialValue)
        {
            return (long) (initialValue + (Value - initialValue) * x);
        }
    }
}