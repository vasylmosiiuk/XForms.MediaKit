using System;
using MediaKit.Core.TypeConverters;
using Xamarin.Forms;

namespace MediaKit.Core
{
    [TypeConverter(typeof(RepeatBehaviorTypeConverter))]
    public struct RepeatBehavior
    {
        private enum RepeatType
        {
            Single,
            Count,
            Duration,
            Forever
        }

        private readonly RepeatType _repeatType;
        private readonly TimeSpan _duration;
        private readonly uint _count;

        private RepeatBehavior(RepeatType repeatType) : this()
        {
            _repeatType = repeatType;
        }

        public RepeatBehavior(TimeSpan duration) : this(RepeatType.Duration)
        {
            _duration = duration;
        }

        public RepeatBehavior(uint count) : this(RepeatType.Count)
        {
            _count = count;
        }

        public bool HasDuration => _repeatType == RepeatType.Duration;
        public bool HasCount => _repeatType == RepeatType.Count;
        public bool RepeatEnabled => _repeatType != RepeatType.Single && (!HasCount || Count > 1);
        public static RepeatBehavior Forever { get; } = new RepeatBehavior(RepeatType.Forever);

        public TimeSpan Duration
        {
            get
            {
                if (HasDuration)
                    return _duration;
                throw new InvalidOperationException("RepeatBehaviour specified without duration");
            }
        }

        public uint Count
        {
            get
            {
                if (HasCount)
                    return _count;
                throw new InvalidOperationException("RepeatBehaviour specified without count");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is RepeatBehavior rb)
                return Equals(rb);
            return base.Equals(obj);
        }

        public bool Equals(RepeatBehavior other)
        {
            if (_repeatType == other._repeatType)
            {
                switch (_repeatType)
                {
                    case RepeatType.Single:
                    case RepeatType.Forever:
                        return true;
                    case RepeatType.Count:
                        return _count == other._count;
                    case RepeatType.Duration:
                        return _duration == other._duration;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) _repeatType;
                hashCode = (hashCode * 397) ^ _duration.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) _count;
                return hashCode;
            }
        }

        public static bool operator ==(RepeatBehavior self, RepeatBehavior other)
        {
            return self.Equals(other);
        }

        public static bool operator !=(RepeatBehavior self, RepeatBehavior other)
        {
            return !self.Equals(other);
        }

        public override string ToString()
        {
            switch (_repeatType)
            {
                case RepeatType.Count:
                    return $"{_count}x times";
                case RepeatType.Duration:
                    return $"{_duration}";
                case RepeatType.Forever:
                    return "Forever";
                case RepeatType.Single:
                    return "Single time";
                default:
                    throw new NotImplementedException("Unknown RepeatBehavior type");
            }
        }
    }
}