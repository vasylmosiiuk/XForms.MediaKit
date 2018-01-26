using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    /// <summary>
    /// KeyFrame animation which proceed <see cref="float"/> values.
    /// </summary>
    public sealed class SingleAnimationUsingKeyFrames : KeyFrameAnimation<float>
    {
    }
    
    /// <summary>
    /// Discrete animation key frame which maintain <see cref="float"/> values.
    /// </summary>
    public sealed class DiscreteSingleKeyFrame : DiscreteKeyFrame<float>
    {
    }
    
    /// <summary>
    /// Linear animation key frame which maintain <see cref="float"/> values.
    /// </summary>
    public sealed class LinearSingleKeyFrame : LinearKeyFrame<float>
    {
        /// <summary>
        ///     Returns interpolated value between <paramref name="initialValue" /> begin and
        ///     <see cref="KeyFrame{TValue}.Value" />, with <paramref name="x" /> as interpolation position.
        /// </summary>
        /// <param name="x">Interpolation position, value in range [0;1].</param>
        /// <param name="initialValue">Interpolation From value.</param>
        /// <returns></returns>
        protected override float GetInterpolatedValue(double x, float initialValue)
        {
            return (float) (initialValue + (Value - initialValue) * x);
        }
    }
}
