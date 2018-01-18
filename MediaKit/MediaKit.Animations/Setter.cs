using System;
using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Animations
{
    /// <summary>
    ///     Special animation type which immediately applies value to <see cref="TargetProperty" /> with delay, specified by
    ///     <see cref="BeginTime" />.
    /// </summary>
    /// <typeparam name="TValue">
    ///     Type of applied value, is required by XAML infrastructure to correctly resolve
    ///     <see cref="TypeConverter" /> for XAML value. You, also may specify it with <see cref="object" />, but you should
    ///     explicitly specify type of <see cref="Value" /> in XAML.
    /// </typeparam>
    public sealed class Setter<TValue> : IAnimation
    {
        private readonly Duration _duration = new Duration(TimeSpan.Zero);
        private readonly RepeatBehavior _repeatBehavior = new RepeatBehavior();
        private TimeSpan _beginTime;
        private Easing _easing = Easing.Linear;
        private FillBehavior _fillBehavior = FillBehavior.HoldEnd;
        private bool _isApplied;
        private BindableObject _target;
        private BindableProperty _targetProperty;
        private TValue _value;

        /// <summary>
        /// Value, which will be applied to <see cref="TargetProperty"/> of target.
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
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
        ///     <see cref="Core.FillBehavior" /> strategy of restoring <see cref="IAnimation.TargetProperty" /> after finishing animation
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
            set => throw new InvalidOperationException($"{nameof(Duration)} can't be changed");
        }

        /// <summary>
        ///     <see cref="Core.RepeatBehavior" /> strategy of repeating animation. Is read-only property.
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, you try to set something.</exception>
        public RepeatBehavior RepeatBehavior
        {
            get => _repeatBehavior;
            set => throw new InvalidOperationException($"{nameof(AutoReverse)} isn't supported");
        }

        /// <summary>
        ///     Specifies, does animation 'backs' after finishing single step. Is read-only property.
        /// </summary>
        /// <exception cref="InvalidOperationException">In case, you try to set something.</exception>
        public bool AutoReverse
        {
            get => false;
            set => throw new InvalidOperationException($"{nameof(AutoReverse)} isn't supported");
        }

        /// <summary>
        ///     Flag, specifies, this object is in freezed state.
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

        /// <summary>
        ///     Method, executed before starting storyboard animations, used for storing original data before any animations will
        ///     be applied
        /// </summary>
        /// <param name="target"><see cref="BindableObject" />target bindable, which will be changed in scope of animation</param>
        /// <returns>Specific value, which holds some data, used by this animation</returns>
        public object StoreAnimationState(BindableObject target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            return new State(target, target.GetValue(TargetProperty));
        }

        /// <summary>
        ///     Method, executed each animation step
        /// </summary>
        /// <param name="x">Current X [0;1] value</param>
        /// <param name="stateRaw">Animation state object, returned from <see cref="IAnimation.StoreAnimationState" /></param>
        public void Update(double x, object stateRaw)
        {
            if (stateRaw is State state)
            {
                if (Math.Abs(x - 1.0) < double.Epsilon)
                {
                    state.Target.SetValue(TargetProperty, Value);
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"State ({stateRaw?.GetType()}), specified for restore isn't compatible with this animation");
            }
        }

        /// <summary>
        ///     Method, executed when animation finished execution
        /// </summary>
        /// <param name="target"><see cref="BindableObject" />target bindable, changed in scope of animation</param>
        /// <param name="stateRaw">Animation state object, returned from <see cref="IAnimation.StoreAnimationState" /></param>
        public void RestoreAnimationState(BindableObject target, object stateRaw)
        {
            if (stateRaw == null || target == null) return;
            if (stateRaw is State state)
            {
                if (state.Target == target)
                {
                    if (FillBehavior == FillBehavior.Stop)
                        target.SetValue(TargetProperty, state.StoredValue);
                }
                else
                    throw new InvalidOperationException(
                        $"Target ({target.GetType()}), specified for restore isn't same with original target ({state.Target.GetType()})");
            }
            else
            {
                throw new InvalidOperationException(
                    $"State ({stateRaw.GetType()}), specified for restore isn't compatible with this animation");
            }
        }

        /// <summary>
        ///     Method, which switches this object into freezed state
        /// </summary>
        /// <exception cref="InvalidOperationException">In case this object is freezed right now</exception>
        public void Apply()
        {
            IsApplied = true;
        }

        private class State
        {
            public readonly object StoredValue;
            public readonly BindableObject Target;

            public State(BindableObject target, object storedValue)
            {
                Target = target;
                StoredValue = storedValue;
            }
        }
    }
}