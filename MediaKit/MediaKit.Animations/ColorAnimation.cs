using System;
using MediaKit.Animations.Abstractions;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Animations
{
    public class ColorAnimation : LinearPrimitiveAnimation<Color>
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

        protected override void Update(double x, LinearAnimationState<Color> state)
        {
            var a = InterpolateAlpha ? state.From.A + (To.A - state.From.A) * x : state.From.A;
            var r = state.From.R + (To.R - state.From.R) * x;
            var g = state.From.G + (To.G - state.From.G) * x;
            var b = state.From.B + (To.B - state.From.B) * x;

            FixColorComponent(ref r);
            FixColorComponent(ref g);
            FixColorComponent(ref b);
            FixColorComponent(ref a);
            var value = Color.FromRgba(r, g, b, a);
            state.Target.SetValue(TargetProperty, value);
        }

        private void FixColorComponent(ref double value) => value = Math.Max(0, Math.Min(1.0, value));
    }
}