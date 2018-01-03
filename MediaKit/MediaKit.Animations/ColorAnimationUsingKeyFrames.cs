using System;
using MediaKit.Animations.Abstractions;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Animations
{
    public class ColorAnimationUsingKeyFrames : KeyFrameAnimation<Color>
    {
    }

    public class DiscreteColorKeyFrame : DiscreteKeyFrame<Color>
    {
    }

    public class LinearColorKeyFrame : LinearKeyFrame<Color>
    {
        private bool _interpolateAlpha;

        public bool InterpolateAlpha
        {
            get => _interpolateAlpha;
            set
            {
                this.ThrowIfApplied();
                _interpolateAlpha = value;
            }
        }

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