using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    /// <summary>
    /// KeyFrame animation which proceed <see cref="short"/> values.
    /// </summary>
    public sealed class Int16AnimationUsingKeyFrames : KeyFrameAnimation<short>
    {
    }

    /// <summary>
    /// Discrete animation key frame which maintain <see cref="short"/> values.
    /// </summary>
    public sealed class DiscreteInt16KeyFrame : DiscreteKeyFrame<short>
    {
    }

    /// <summary>
    /// Linear animation key frame which maintain <see cref="short"/> values.
    /// </summary>
    public sealed class LinearInt16KeyFrame : LinearKeyFrame<short>
    {
        /// <summary>
        ///     Returns interpolated value between <paramref name="initialValue" /> begin and
        ///     <see cref="KeyFrame{TValue}.Value" />, with <paramref name="x" /> as interpolation position.
        /// </summary>
        /// <param name="x">Interpolation position, value in range [0;1].</param>
        /// <param name="initialValue">Interpolation From value.</param>
        /// <returns></returns>
        protected override short GetInterpolatedValue(double x, short initialValue)
        {
            return (short) (initialValue + (Value - initialValue) * x);
        }
    }
}