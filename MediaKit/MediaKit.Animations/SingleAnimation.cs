using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    public sealed class SingleAnimation : LinearPrimitiveAnimation<float>
    {
        protected override void Update(double x, LinearAnimationState<float> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, (float)value);
        }
    }
}