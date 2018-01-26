using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    /// <summary>
    ///     KeyFrame animation which proceed <see cref="double" /> values.
    /// </summary>
    public sealed class DoubleAnimationUsingKeyFrames : KeyFrameAnimation<double>
    {
    }

    /// <summary>
    ///     Discrete animation key frame which maintain <see cref="double" /> values.
    /// </summary>
    public sealed class DiscreteDoubleKeyFrame : DiscreteKeyFrame<double>
    {
    }

    /// <summary>
    ///     Linear animation key frame which maintain <see cref="double" /> values.
    /// </summary>
    public sealed class LinearDoubleKeyFrame : LinearKeyFrame<double>
    {
        /// <summary>
        ///     Returns interpolated value between <paramref name="initialValue" /> begin and
        ///     <see cref="KeyFrame{TValue}.Value" />, with <paramref name="x" /> as interpolation position.
        /// </summary>
        /// <param name="x">Interpolation position, value in range [0;1].</param>
        /// <param name="initialValue">Interpolation From value.</param>
        /// <returns></returns>
        protected override double GetInterpolatedValue(double x, double initialValue)
        {
            return initialValue + (Value - initialValue) * x;
        }
    }
}