using System;
using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Animations.Abstractions
{
    /// <summary>
    ///     Base class for MediaKit built-in animations, you may use it as a base for your custom animations.
    /// </summary>
    /// <typeparam name="TState">The type of animation state storage object.</typeparam>
    public abstract class AnimationBase<TState> : IAnimation
        where TState : AnimationState
    {
        private bool _autoReverse;
        private TimeSpan _beginTime = TimeSpan.Zero;
        private Duration _duration = new Duration(TimeSpan.Zero);
        private Easing _easing = Easing.Linear;
        private FillBehavior _fillBehavior = FillBehavior.HoldEnd;
        private bool _isApplied;
        private RepeatBehavior _repeatBehavior;
        private BindableObject _target;
        private BindableProperty _targetProperty;

        /// <summary>
        ///     <see cref="Core.FillBehavior" /> strategy of restoring <see cref="IAnimation.TargetProperty" /> after finishing
        ///     animation
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public FillBehavior FillBehavior
        {
            get => _fillBehavior;
            set
            {
                this.ThrowIfApplied();
                _fillBehavior = value;
            }
        }

        /// <summary>
        ///     <see cref="Xamarin.Forms.Easing" /> object which modifies X value
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public Easing Easing
        {
            get => _easing;
            set
            {
                this.ThrowIfApplied();
                _easing = value;
            }
        }

        /// <summary>
        ///     <see cref="TimeSpan" /> delay before animation started in scope of <see cref="Storyboard" />
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public TimeSpan BeginTime
        {
            get => _beginTime;
            set
            {
                this.ThrowIfApplied();
                _beginTime = value;
            }
        }

        /// <summary>
        ///     <see cref="Core.Duration" /> of animation execution
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public Duration Duration
        {
            get => _duration;
            set
            {
                this.ThrowIfApplied();
                _duration = value;
            }
        }

        /// <summary>
        ///     Explicit target bindable, so not only <see cref="VisualElement" /> allowed, but any <see cref="BindableObject" />
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public BindableObject Target
        {
            get => _target;
            set
            {
                this.ThrowIfApplied();
                _target = value;
            }
        }

        /// <summary>
        ///     <see cref="BindableProperty" />, which is changeable
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public BindableProperty TargetProperty
        {
            get => _targetProperty;
            set
            {
                this.ThrowIfApplied();
                _targetProperty = value;
            }
        }

        /// <summary>
        ///     <see cref="Core.RepeatBehavior" /> strategy of repeating animation
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public RepeatBehavior RepeatBehavior
        {
            get => _repeatBehavior;
            set
            {
                this.ThrowIfApplied();
                _repeatBehavior = value;
            }
        }

        /// <summary>
        ///     Specifies, does animation 'backs' after finishing single step
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public bool AutoReverse
        {
            get => _autoReverse;
            set
            {
                this.ThrowIfApplied();
                _autoReverse = value;
            }
        }

        void IAnimation.RestoreAnimationState(BindableObject target, object stateRaw)
        {
            if (stateRaw == null || target == null) return;
            if (stateRaw is TState state)
                if (state.Target == target)
                    RestoreAnimationState(target, state);
                else
                    throw new InvalidOperationException(
                        $"Target ({target.GetType()}), specified for restore isn't same with original target ({state.Target.GetType()})");
            else
                throw new InvalidOperationException(
                    $"State ({stateRaw.GetType()}), specified for restore isn't compatible with this animation");
        }

        void IAnimation.Update(double x, object stateRaw)
        {
            if (stateRaw == null) Update(x, null);
            if (stateRaw is TState state)
                Update(x, state);
            else
                throw new InvalidOperationException(
                    $"State ({stateRaw?.GetType()}), specified for restore isn't compatible with this animation");
        }

        object IAnimation.StoreAnimationState(BindableObject target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            return StoreAnimationState(target);
        }

        /// <summary>
        ///     Flag, specifies, this object is in freezed state
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public bool IsApplied
        {
            get => _isApplied;
            private set
            {
                this.ThrowIfApplied();
                _isApplied = value;
            }
        }

        void IApplicable.Apply()
        {
            Apply();
            IsApplied = true;
        }

        /// <summary>
        ///     Should be implemented in descendents to execute some logic before animation will be switched into freezed state.
        /// </summary>
        protected virtual void Apply()
        {
        }

        /// <summary>
        ///     Method, executed when animation finished execution.
        /// </summary>
        /// <param name="target"><see cref="BindableObject" />target bindable, changed in scope of animation.</param>
        /// <param name="state">Animation state object, created by <see cref="StoreAnimationState" /> and returned.</param>
        protected virtual void RestoreAnimationState(BindableObject target, TState state)
        {
        }

        /// <summary>
        ///     Method, executed before starting storyboard animations, used for storing original data before any animations will
        ///     be applied.
        /// </summary>
        /// <param name="target"><see cref="BindableObject" />target bindable, which will be changed in scope of animation</param>
        /// <returns>Specific value, which holds some data, used by this animation</returns>
        protected virtual TState StoreAnimationState(BindableObject target)
        {
            return default(TState);
        }

        /// <summary>
        ///     Method, executed each animation step.
        /// </summary>
        /// <param name="x">Current X [0;1] value.</param>
        /// <param name="state">Animation state object, created by <see cref="StoreAnimationState" /> and returned.</param>
        protected abstract void Update(double x, TState state);
    }
}