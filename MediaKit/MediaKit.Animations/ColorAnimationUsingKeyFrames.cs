using System;
using MediaKit.Animations.Abstractions;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Animations
{
    /// <summary>
    /// KeyFrame animation which proceed <see cref="Color"/> values.
    /// </summary>
    public sealed class ColorAnimationUsingKeyFrames : KeyFrameAnimation<Color>
    {
    }

    /// <summary>
    /// Discrete animation key frame which maintain <see cref="Color"/> values.
    /// </summary>
    public sealed class DiscreteColorKeyFrame : DiscreteKeyFrame<Color>
    {
    }


    /// <summary>
    /// Linear animation key frame which maintain <see cref="Color"/> values.
    /// </summary>
    public sealed class LinearColorKeyFrame : LinearKeyFrame<Color>
    {
        private bool _interpolateAlpha;

        /// <summary>
        /// Flag, marks should we interpolating <see cref="Color.A"/> also, or stay it as it was in <see cref="KeyFrameAnimationState{T}.StoredValue"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, you try to set something.</exception>
        public bool InterpolateAlpha
        {
            get => _interpolateAlpha;
            set
            {
                this.ThrowIfApplied();
                _interpolateAlpha = value;
            }
        }

        /// <summary>
        ///     Returns interpolated value between <paramref name="initialValue" /> begin and
        ///     <see cref="KeyFrame{TValue}.Value" />, with <paramref name="x" /> as interpolation position.
        /// </summary>
        /// <param name="x">Interpolation position, value in range [0;1].</param>
        /// <param name="initialValue">Interpolation From value.</param>
        /// <returns></returns>
        protected override Color GetInterpolatedValue(double x, Color initialValue)
        {
            var a = InterpolateAlpha ? initialValue.A + (Value.A - initialValue.A) * x : initialValue.A;
            var r = initialValue.R + (Value.R - initialValue.R) * x;
            var g = initialValue.G + (Value.G - initialValue.G) * x;
            var b = initialValue.B + (Value.B - initialValue.B) * x;

            FixColorComponent(ref r);
            FixColorComponent(ref g);
            FixColorComponent(ref b);
            FixColorComponent(ref a);
            return Color.FromRgba(r, g, b, a);
        }

        private void FixColorComponent(ref double value) => value = Math.Max(0, Math.Min(1.0, value));
    }
}