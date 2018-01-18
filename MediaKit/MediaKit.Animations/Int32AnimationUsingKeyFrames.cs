using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    /// <summary>
    ///     KeyFrame animation which proceed <see cref="int" /> values.
    /// </summary>
    public sealed class Int32AnimationUsingKeyFrames : KeyFrameAnimation<int>
    {
    }

    /// <summary>
    ///     Discrete animation key frame which maintain <see cref="int" /> values.
    /// </summary>
    public sealed class DiscreteInt32KeyFrame : DiscreteKeyFrame<int>
    {
    }

    /// <summary>
    ///     Linear animation key frame which maintain <see cref="int" /> values.
    /// </summary>
    public sealed class LinearInt32KeyFrame : LinearKeyFrame<int>
    {
        /// <summary>
        ///     Returns interpolated value between <paramref name="initialValue" /> begin and
        ///     <see cref="KeyFrame{TValue}.Value" />, with <paramref name="x" /> as interpolation position.
        /// </summary>
        /// <param name="x">Interpolation position, value in range [0;1].</param>
        /// <param name="initialValue">Interpolation From value.</param>
        /// <returns></returns>
        protected override int GetInterpolatedValue(double x, int initialValue) =>
            (int) (initialValue + (Value - initialValue) * x);
    }
}