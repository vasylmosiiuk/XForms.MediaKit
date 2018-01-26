using System;
using MediaKit.Core;

namespace MediaKit.Animations.Abstractions
{
    /// <summary>
    /// The contract, which declares that descendant should provide some value and specific time. Used by KeyFrame animations.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public interface IKeyFrame<TValue> : IApplicable
    {
        /// <summary>
        /// Timeline offset, when <see cref="Value"/> should be started applying.
        /// </summary>
        TimeSpan KeyTime { get; set; }

        /// <summary>
        /// Value, which should be used.
        /// </summary>
        TValue Value { get; set; }

        /// <summary>
        ///     Should be implemented in descendants and contains some <see cref="KeyFrameAnimationState{T}.TargetProperty" />
        ///     updation logic.
        /// </summary>
        /// <param name="x">
        ///     [0;1] value, describes KeyFrame's owner animation position, calculated based on <see cref="IAnimation.BeginTime" />,
        ///     <see cref="IAnimation.Duration" /> and current animation's position.
        /// </param>
        /// <param name="state"><see cref="KeyFrameAnimation{TAnimationValue}"/> animation state object.</param>
        void Update(double x, KeyFrameAnimationState<TValue> state);
    }
}