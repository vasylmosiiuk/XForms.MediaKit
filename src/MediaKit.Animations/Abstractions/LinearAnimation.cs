using MediaKit.Core;
using Xamarin.Forms;

namespace MediaKit.Animations.Abstractions
{
    /// <summary>
    ///     Base class for linear animations (which change <see cref="IAnimation.TargetProperty" /> with value, interpolated
    ///     beween <see cref="From" /> and <see cref="To" /> values.
    ///     Used by MediaKit built-in animations, you may use it incustom animations.
    /// </summary>
    /// <typeparam name="TAnimationValue">Type of value.</typeparam>
    public abstract class LinearAnimation<TAnimationValue> : AnimationBase<LinearAnimationState<TAnimationValue>>
        where TAnimationValue : class
    {
        /// <summary>
        ///     Begin value.
        /// </summary>
        public TAnimationValue From { get; set; }

        /// <summary>
        ///     End value.
        /// </summary>
        public TAnimationValue To { get; set; }

        /// <summary>
        ///     Method, executed before starting storyboard animations, used for storing original data before any animations will
        ///     be applied.
        /// </summary>
        /// <param name="target"><see cref="BindableObject" />target bindable, which will be changed in scope of animation</param>
        /// <returns>Specific value, which holds some data, used by this animation</returns>
        protected override LinearAnimationState<TAnimationValue> StoreAnimationState(BindableObject target)
        {
            var currentValue = (TAnimationValue) target.GetValue(TargetProperty);
            return new LinearAnimationState<TAnimationValue>(target, currentValue, From ?? currentValue);
        }

        /// <summary>
        ///     Method, executed when animation finished execution.
        /// </summary>
        /// <param name="target"><see cref="BindableObject" />target bindable, changed in scope of animation.</param>
        /// <param name="state">
        ///     Animation state object, created by <see cref="AnimationBase{TState}.StoreAnimationState" /> and
        ///     returned.
        /// </param>
        protected override void RestoreAnimationState(BindableObject target,
            LinearAnimationState<TAnimationValue> state)
        {
            if (FillBehavior == FillBehavior.Stop)
            {
                state.Target.SetValue(TargetProperty, state.StoredValue);
            }
        }
    }

    /// <summary>
    ///     Base class for linear animations (which change <see cref="IAnimation.TargetProperty" /> with value, interpolated
    ///     beween <see cref="From" /> and <see cref="To" /> values.
    ///     Used by MediaKit built-in animations, you may use it incustom animations.
    /// </summary>
    /// <typeparam name="TAnimationValue">Type of value.</typeparam>
    public abstract class
        LinearPrimitiveAnimation<TAnimationValue> : AnimationBase<LinearAnimationState<TAnimationValue>>
        where TAnimationValue : struct
    {
        /// <summary>
        ///     Begin value.
        /// </summary>
        public TAnimationValue? From { get; set; }

        /// <summary>
        ///     End value.
        /// </summary>
        public TAnimationValue To { get; set; }

        /// <summary>
        ///     Method, executed before starting storyboard animations, used for storing original data before any animations will
        ///     be applied.
        /// </summary>
        /// <param name="target"><see cref="BindableObject" />target bindable, which will be changed in scope of animation</param>
        /// <returns>Specific value, which holds some data, used by this animation</returns>
        protected override LinearAnimationState<TAnimationValue> StoreAnimationState(BindableObject target)
        {
            var currentValue = (TAnimationValue) target.GetValue(TargetProperty);
            return new LinearAnimationState<TAnimationValue>(target, currentValue, From ?? currentValue);
        }

        /// <summary>
        ///     Method, executed when animation finished execution.
        /// </summary>
        /// <param name="target"><see cref="BindableObject" />target bindable, changed in scope of animation.</param>
        /// <param name="state">Animation state object, created by <see cref="AnimationBase{TState}.StoreAnimationState" /> and returned.</param>
        protected override void RestoreAnimationState(BindableObject target,
            LinearAnimationState<TAnimationValue> state)
        {
            if (FillBehavior == FillBehavior.Stop)
            {
                state.Target.SetValue(TargetProperty, state.StoredValue);
            }
        }
    }
}