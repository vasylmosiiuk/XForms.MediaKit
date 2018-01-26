using System;
using MediaKit.Core.TypeConverters;
using Xamarin.Forms;

namespace MediaKit.Core
{
    /// <summary>
    ///     Specifies animation repeat strategy after single loop finish.
    /// </summary>
    [TypeConverter(typeof(RepeatBehaviorTypeConverter))]
    public struct RepeatBehavior
    {
        private enum RepeatType
        {
            Single,
            Times,
            Duration,
            Forever
        }

        private readonly RepeatType _repeatType;
        private readonly TimeSpan _duration;
        private readonly uint _times;

        private RepeatBehavior(RepeatType repeatType) : this()
        {
            _repeatType = repeatType;
        }

        /// <summary>
        ///     Initializes <see cref="RepeatBehavior" /> with timespan. Means that animation will be repeating by time specified,
        ///     so it possible that animation first or last loop will be executed partially.
        /// </summary>
        /// <param name="duration"><see cref="TimeSpan" /> value, which specifies repeat duration.</param>
        public RepeatBehavior(TimeSpan duration) : this(RepeatType.Duration)
        {
            _duration = duration;
        }

        /// <summary>
        ///     Initializes <see cref="RepeatBehavior" /> with times value. Means that animation will be repeating by times
        ///     specified,
        ///     so 1x times value defines animation without repeating, 2x and more - animation will be repeating n-
        ///     <paramref name="times" />.
        /// </summary>
        /// <param name="times">Count of times, animation will be repeated.</param>
        public RepeatBehavior(uint times) : this(RepeatType.Times)
        {
            _times = times;
        }

        /// <summary>
        ///     Flag, which marks is this <see cref="RepeatBehavior" /> initialized with <see cref="TimeSpan" /> value.
        /// </summary>
        public bool HasDuration => _repeatType == RepeatType.Duration;

        /// <summary>
        ///     Flag, which marks is this <see cref="RepeatBehavior" /> initialized with repeating times count value.
        /// </summary>
        public bool HasTimes => _repeatType == RepeatType.Times;

        /// <summary>
        ///     Flag, which marks is this <see cref="RepeatBehavior" /> specifies animation repeating strategy with 2x or more
        ///     times count or some <see cref="TimeSpan" /> value.
        /// </summary>
        public bool RepeatEnabled => _repeatType != RepeatType.Single && (!HasTimes || Times > 1);

        /// <summary>
        ///     Predefined <see cref="RepeatBehavior" /> strategy for forever times animation repeating.
        /// </summary>
        public static RepeatBehavior Forever { get; } = new RepeatBehavior(RepeatType.Forever);

        /// <summary>
        ///     <see cref="TimeSpan" /> value, defined by specific constructor.
        /// </summary>
        /// <exception cref="InvalidOperationException">In case if this strategy wasn't defined with <see cref="TimeSpan" /> value.</exception>
        public TimeSpan Duration
        {
            get
            {
                if (HasDuration)
                    return _duration;
                throw new InvalidOperationException("RepeatBehaviour specified without duration");
            }
        }

        /// <summary>
        ///     Repeat times value, defined by specific constructor.
        /// </summary>
        /// <exception cref="InvalidOperationException">In case if this strategy wasn't defined with <see cref="uint" /> value.</exception>
        public uint Times
        {
            get
            {
                if (HasTimes)
                    return _times;
                throw new InvalidOperationException("RepeatBehaviour specified without count of times");
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
                    case RepeatType.Times:
                        return _times == other._times;
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
                hashCode = (hashCode * 397) ^ (int) _times;
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
                case RepeatType.Times:
                    return $"{_times}x times";
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        ///     Parses plain string value into <see cref="RepeatBehavior" />.
        /// </summary>
        /// <param name="value"><see cref="string" /> value. Permitted values and formats: Forever, {uint}x, {TimeSpan}.</param>
        /// <returns>
        ///     <see cref="RepeatBehavior" /> result or <see cref="InvalidOperationException" /> in case impossible to parse
        ///     input string.
        /// </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static RepeatBehavior Parse(string value)
        {
            value = value?.Trim() ?? "";
            if (string.IsNullOrEmpty(value))
                return new RepeatBehavior();
            if (string.Compare(value, "Forever", StringComparison.OrdinalIgnoreCase) == 0)
                return Forever;
            if (value.Length > 1 && value.EndsWith("x") &&
                uint.TryParse(value.Substring(0, value.Length - 1), out var count))
                return new RepeatBehavior(count);
            if (TimeSpan.TryParse(value, out var timeSpan))
                return new RepeatBehavior(timeSpan);

            throw new InvalidOperationException(
                $"Impossible to parse string \"{value}\" to RepeatBehavior, possible values are: Forever, timespan or %Times%x values");
        }

        /// <summary>
        /// Safety parses plain string value into <see cref="RepeatBehavior"/>
        /// </summary>
        /// <param name="value"><see cref="string" /> value. Permitted values and formats: Forever, {uint}x, {TimeSpan}.</param>
        /// <param name="repeatBehavior">resulting value in case if return value is true.</param>
        /// <returns>true if parse operation successfull, false is parse exception was occured.</returns>
        public static bool TryParse(string value, out RepeatBehavior repeatBehavior)
        {
            try
            {
                repeatBehavior = Parse(value);
                return true;
            }
            catch (InvalidOperationException)
            {
                repeatBehavior = default(RepeatBehavior);
                return false;
            }
        }
    }
}