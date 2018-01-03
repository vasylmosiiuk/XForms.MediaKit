using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    public sealed class Int32Animation : LinearPrimitiveAnimation<int>
    {
        protected override void Update(double x, LinearAnimationState<int> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, (int)value);
        }
    }
}