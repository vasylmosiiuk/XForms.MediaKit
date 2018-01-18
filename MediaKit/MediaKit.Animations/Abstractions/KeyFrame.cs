using System;
using MediaKit.Core;
using MediaKit.Core.Extensions;

namespace MediaKit.Animations.Abstractions
{
    /// <summary>
    ///     Base type for KeyFrames used by MediaKit built-in animation. You may extend it or use as is in custom animations.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class KeyFrame<TValue> : IKeyFrame<TValue>
    {
        private bool _isApplied;
        private TimeSpan _keyTime;

        private TValue _value;

        /// <summary>
        ///     Timeline offset, when <see cref="IKeyFrame{TValue}.Value" /> should be started applying.
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, you try to set something.</exception>
        public TimeSpan KeyTime
        {
            get => _keyTime;
            set
            {
                this.ThrowIfApplied();
                _keyTime = value;
            }
        }

        /// <summary>
        ///     Value, which should be used.
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, you try to set something.</exception>
        public TValue Value
        {
            get => _value;
            set
            {
                this.ThrowIfApplied();
                _value = value;
            }
        }

        /// <summary>
        ///     Flag, specifies, this object is in freezed state
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, you try to set something.</exception>
        public bool IsApplied
        {
            get => _isApplied;
            private set
            {
                this.ThrowIfApplied();
                _isApplied = value;
            }
        }

        /// <summary>
        ///     Method, which switches this object into freezed state
        /// </summary>
        /// <exception cref="InvalidOperationException">In case this object is freezed right now</exception>
        public void Apply() => IsApplied = true;

        /// <summary>
        ///     Should be implemented in descendants and contains some <see cref="KeyFrameAnimationState{T}.TargetProperty" />
        ///     updation logic.
        /// </summary>
        /// <param name="x">
        ///     [0;1] value, describes KeyFrame's owner animation position, calculated based on <see cref="IAnimation.BeginTime" />,
        ///     <see cref="IAnimation.Duration" /> and current animation's position.
        /// </param>
        /// <param name="state"><see cref="KeyFrameAnimation{TAnimationValue}"/> animation state object.</param>
        public abstract void Update(double x, KeyFrameAnimationState<TValue> state);
    }
}