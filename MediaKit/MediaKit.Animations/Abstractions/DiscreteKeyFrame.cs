using MediaKit.Core;

namespace MediaKit.Animations.Abstractions
{
    /// <summary>
    ///     Base class for discrete value key frames. It means, the <see cref="KeyFrame{TValue}.Value" /> will be immediately
    ///     applied to <see cref="KeyFrameAnimationState{T}.TargetProperty" /> as is without any interpolations.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public abstract class DiscreteKeyFrame<TValue> : KeyFrame<TValue>
    {
        ///<inheritdoc cref="KeyFrame{TValue}"/>
        /// <summary>
        /// <see cref="AnimationState.Target"/>.<see cref="KeyFrameAnimationState{T}.TargetProperty"/> update logic.
        /// </summary>
        /// <param name="x">
        ///     [0;1] value, describes KeyFrame's owner animation position, calculated based on <see cref="IAnimation.BeginTime" />,
        ///     <see cref="IAnimation.Duration" /> and current animation's position.
        /// </param>
        /// <param name="state"><see cref="KeyFrameAnimation{TAnimationValue}"/> animation state object.</param>
        public override void Update(double x, KeyFrameAnimationState<TValue> state)
        {
            state.Target.SetValue(state.TargetProperty, Value);
        }
    }
}