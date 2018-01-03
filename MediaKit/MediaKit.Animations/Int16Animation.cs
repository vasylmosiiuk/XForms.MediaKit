using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    public sealed class Int16Animation : LinearPrimitiveAnimation<short>
    {
        protected override void Update(double x, LinearAnimationState<short> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, (short)value);
        }
    }
}