using System;
using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Animations.Abstractions
{
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
        private BindableProperty _targetProperty;

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
            set
            {
                this.ThrowIfApplied();
                _duration = value;
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

        public RepeatBehavior RepeatBehavior
        {
            get => _repeatBehavior;
            set
            {
                this.ThrowIfApplied();
                _repeatBehavior = value;
            }
        }

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

        protected virtual void Apply() { }

        protected virtual void RestoreAnimationState(BindableObject target, TState state)
        {
        }

        protected virtual TState StoreAnimationState(BindableObject target)
        {
            return default(TState);
        }

        protected abstract void Update(double x, TState state);
    }
}