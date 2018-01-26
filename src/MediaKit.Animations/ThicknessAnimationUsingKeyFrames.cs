using MediaKit.Animations.Abstractions;
using Xamarin.Forms;

namespace MediaKit.Animations
{
    /// <summary>
    ///     KeyFrame animation which proceed <see cref="Thickness" /> values.
    /// </summary>
    public sealed class ThicknessAnimationUsingKeyFrames : KeyFrameAnimation<Thickness>
    {
    }

    /// <summary>
    ///     Discrete animation key frame which maintain <see cref="Thickness" /> values.
    /// </summary>
    public sealed class DiscreteThicknessKeyFrame : DiscreteKeyFrame<Thickness>
    {
    }
    
    /// <summary>
    ///     Linear animation key frame which maintain <see cref="Thickness" /> values.
    /// </summary>
    public sealed class LinearThicknessKeyFrame : LinearKeyFrame<Thickness>
    {
        /// <summary>
        ///     Returns interpolated value between <paramref name="initialValue" /> begin and
        ///     <see cref="KeyFrame{TValue}.Value" />, with <paramref name="x" /> as interpolation position.
        /// </summary>
        /// <param name="x">Interpolation position, value in range [0;1].</param>
        /// <param name="initialValue">Interpolation From value.</param>
        /// <returns></returns>
        protected override Thickness GetInterpolatedValue(double x, Thickness initialValue)
        {
            var left = initialValue.Left + (Value.Left - initialValue.Left) * x;
            var right = initialValue.Right + (Value.Right - initialValue.Right) * x;
            var top = initialValue.Top + (Value.Top - initialValue.Top) * x;
            var bottom = initialValue.Bottom + (Value.Bottom - initialValue.Bottom) * x;

            return new Thickness(left, top, right, bottom);
        }
    }
}