using System;
using MediaKit.Core.Extensions;

namespace MediaKit.Animations.Abstractions
{
    public abstract class KeyFrame<TValue> : IKeyFrame<TValue>
    {
        private bool _isApplied;
        private TimeSpan _keyTime;

        private TValue _value;

        public TimeSpan KeyTime
        {
            get => _keyTime;
            set
            {
                this.ThrowIfApplied();
                _keyTime = value;
            }
        }

        public TValue Value
        {
            get => _value;
            set
            {
                this.ThrowIfApplied();
                _value = value;
            }
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

        public void Apply() => IsApplied = true;

        public abstract void Update(double x, KeyFrameAnimationState<TValue> state);
    }
}