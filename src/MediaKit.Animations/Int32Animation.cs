using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    /// <summary>
    /// KeyFrame animation which proceed <see cref="int"/> values.
    /// </summary>
    public sealed class Int32Animation : LinearPrimitiveAnimation<int>
    {
        /// <summary>
        ///     Method, executed each animation step.
        /// </summary>
        /// <param name="x">Current X [0;1] value.</param>
        /// <param name="state">Animation state object, created by <see cref="AnimationBase{TState}.StoreAnimationState" /> and returned.</param>
        protected override void Update(double x, LinearAnimationState<int> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, (int)value);
        }
    }
}