using System;
using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Animations
{
    public class Setter<TValue> : IAnimation
    {
        private readonly Duration _duration = new Duration(TimeSpan.Zero);
        private readonly RepeatBehavior _repeatBehavior = new RepeatBehavior();
        private TimeSpan _beginTime;
        private Easing _easing = Easing.Linear;
        private FillBehavior _fillBehavior = FillBehavior.HoldEnd;
        private bool _isApplied;
        private BindableProperty _targetProperty;
        private TValue _value;

        public TValue Value
        {
            get => _value;
            set
            {
                this.ThrowIfApplied();
                _value = value;
            }
        }

        public BindableProperty TargetProperty
        {
            get => _targetProperty;
            set
            {
                this.ThrowIfApplied();
                _targetProperty = value;
            }
        }

        public FillBehavior FillBehavior
        {
            get => _fillBehavior;
            set
            {
                this.ThrowIfApplied();
                _fillBehavior = value;
            }
        }

        public Easing Easing
        {
            get => _easing;
            set
            {
                this.ThrowIfApplied();
                _easing = value;
            }
        }

        public TimeSpan BeginTime
        {
            get => _beginTime;
            set
            {
                this.ThrowIfApplied();
                _beginTime = value;
            }
        }

        public Duration Duration
        {
            get => _duration;
            set => throw new InvalidOperationException($"{nameof(Duration)} can't be changed");
        }

        public RepeatBehavior RepeatBehavior
        {
            get => _repeatBehavior;
            set => throw new InvalidOperationException($"{nameof(AutoReverse)} isn't supported");
        }

        public bool AutoReverse
        {
            get => false;
            set => throw new InvalidOperationException($"{nameof(AutoReverse)} isn't supported");
        }

        public bool IsApplied
        {
            get => _isApplied;
            private set
            {
                this.ThrowIfApplied();
                _isApplied = value;
            }
        }

        public object StoreAnimationState(BindableObject target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            return new State(target, target.GetValue(TargetProperty));
        }

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