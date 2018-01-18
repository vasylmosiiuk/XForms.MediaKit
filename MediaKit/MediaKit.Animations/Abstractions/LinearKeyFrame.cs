using MediaKit.Core;

namespace MediaKit.Animations.Abstractions
{
    /// <summary>
    ///     Base KeyFrame type used by <see cref="KeyFrameAnimation{TAnimationValue}" /> to interpolate
    ///     <see cref="AnimationBase{TState}.TargetProperty" /> value, used in MediaKit built-in animations.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public abstract class LinearKeyFrame<TValue> : KeyFrame<TValue>
    {
        /// <summary>
        ///     Should be implemented in descendants and contains some <see cref="KeyFrameAnimationState{T}.TargetProperty" />
        ///     updation logic.
        /// </summary>
        /// <param name="x">
        ///     [0;1] value, describes KeyFrame's owner animation position, calculated based on <see cref="IAnimation.BeginTime" />,
        ///     <see cref="IAnimation.Duration" /> and current animation's position.
        /// </param>
        /// <param name="state"><see cref="KeyFrameAnimation{TAnimationValue}" /> animation state object.</param>
        public override void Update(double x, KeyFrameAnimationState<TValue> state)
        {
            state.Target.SetValue(state.TargetProperty, GetInterpolatedValue(x, state.PreviousKeyFrameValue));
        }

        /// <summary>
        ///     Returns interpolated value between <paramref name="initialValue" /> begin and
        ///     <see cref="KeyFrame{TValue}.Value" />, with <paramref name="x" /> as interpolation position.
        /// </summary>
        /// <param name="x">Interpolation position, value in range [0;1].</param>
        /// <param name="initialValue">Interpolation From value.</param>
        /// <returns></returns>
        protected abstract TValue GetInterpolatedValue(double x, TValue initialValue);
    }
}