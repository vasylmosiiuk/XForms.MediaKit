using MediaKit.Animations.Abstractions;
using Xamarin.Forms;

namespace MediaKit.Animations
{
    /// <summary>
    /// Linear animation which proceed <see cref="Thickness"/> values.
    /// </summary>
    public sealed class ThicknessAnimation : LinearPrimitiveAnimation<Thickness>
    {
        /// <summary>
        ///     Method, executed each animation step.
        /// </summary>
        /// <param name="x">Current X [0;1] value.</param>
        /// <param name="state">Animation state object, created by <see cref="AnimationBase{TState}.StoreAnimationState" /> and returned.</param>
        protected override void Update(double x, LinearAnimationState<Thickness> state)
        {
            var from = state.From;
            var to = To;
            var left = from.Left + (to.Left - from.Left) * x;
            var right = from.Right + (to.Right - from.Right) * x;
            var top = from.Top + (to.Top - from.Top) * x;
            var bottom = from.Bottom + (to.Bottom - from.Bottom) * x;

            var value = new Thickness(left, top, right, bottom);
            state.Target.SetValue(TargetProperty, value);
        }
    }
}