using System;
using System.Collections.Generic;
using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Animations.Abstractions
{
    /// <summary>
    ///     Base class, used by MediaKit built-in animations to handle KeyFrames based animations.
    /// </summary>
    /// <typeparam name="TAnimationValue">Type of value.</typeparam>
    [ContentProperty(nameof(KeyFrames))]
    public abstract class KeyFrameAnimation<TAnimationValue> : AnimationBase<KeyFrameAnimationState<TAnimationValue>>
    {
        /// <summary>
        ///     KeyFrames, used by this animation.
        /// </summary>
        public KeyFrameCollection<TAnimationValue> KeyFrames { get; } = new KeyFrameCollection<TAnimationValue>();

        /// <summary>
        ///     Method, executed before starting storyboard animations, used for storing original data before any animations will
        ///     be applied.
        /// </summary>
        /// <param name="target"><see cref="BindableObject" />target bindable, which will be changed in scope of animation</param>
        /// <returns>Specific value, which holds some data, used by this animation</returns>
        protected override KeyFrameAnimationState<TAnimationValue> StoreAnimationState(BindableObject target)
        {
            var currentValue = (TAnimationValue) target.GetValue(TargetProperty);
            return new KeyFrameAnimationState<TAnimationValue>(target, TargetProperty, currentValue);
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
            KeyFrameAnimationState<TAnimationValue> state)
        {
            if (FillBehavior == FillBehavior.Stop)
            {
                state.Target.SetValue(TargetProperty, state.StoredValue);
            }
        }

        /// <summary>
        ///     Should be implemented in descendents to execute some logic before animation will be switched into freezed state.
        /// </summary>
        protected override void Apply()
        {
            base.Apply();

            KeyFrames.Sort((x1, x2) => TimeSpan.Compare(x1.KeyTime, x2.KeyTime));
            KeyFrames.ForEach(x => x.Apply());
        }

        /// <summary>
        ///     Method, executed each animation step.
        /// </summary>
        /// <param name="xGlobal">Current X [0;1] value.</param>
        /// <param name="state">
        ///     Animation state object, created by <see cref="AnimationBase{TState}.StoreAnimationState" /> and
        ///     returned.
        /// </param>
        protected override void Update(double xGlobal, KeyFrameAnimationState<TAnimationValue> state)
        {
            var animationDuration = Duration.ToTimeSpan().TotalMilliseconds;
            for (var i = 0; i < KeyFrames.Count; i++)
            {
                var keyFrame = KeyFrames[i];
                var x1 = keyFrame.KeyTime.TotalMilliseconds / animationDuration;
                if (xGlobal < x1) continue;
                if (i == 1)
                {
                }

                var x2 = (i < KeyFrames.Count - 1
                             ? KeyFrames[i + 1].KeyTime.TotalMilliseconds
                             : animationDuration) / animationDuration;
                if (xGlobal > x2) continue;
                var x = (xGlobal - x1) / (x2 - x1);

                state.PreviousKeyFrameValue = i > 0 ? KeyFrames[i - 1].Value : state.StoredValue;

                keyFrame.Update(x, state);
            }
        }
    }

    /// <summary>
    ///     Custom collection type, used to store <see cref="KeyFrame{TValue}" /> sequence.
    /// </summary>
    /// <typeparam name="TValue">Type of value, used for <see cref="IAnimation.TargetProperty" /> updates.</typeparam>
    public class KeyFrameCollection<TValue> : List<KeyFrame<TValue>>
    {
    }
}