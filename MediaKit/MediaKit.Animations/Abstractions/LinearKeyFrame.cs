namespace MediaKit.Animations.Abstractions
{
    public abstract class LinearKeyFrame<TValue> : KeyFrame<TValue>
    {
        public override void Update(double x, KeyFrameAnimationState<TValue> state)
        {
            state.Target.SetValue(state.TargetProperty, GetInterpolatedValue(x, state.PreviousKeyFrameValue));
        }

        protected abstract TValue GetInterpolatedValue(double x, TValue initialValue);
    }
}