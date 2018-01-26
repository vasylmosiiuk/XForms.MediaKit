using System;
using MediaKit.Core.TypeConverters;
using Xamarin.Forms;

namespace MediaKit.Core
{
    /// <summary>
    ///     Structure, specifies duration of animation execution
    /// </summary>
    [TypeConverter(typeof(DurationTypeConverter))]
    public struct Duration
    {
        private enum DurationType
        {
            Auto,
            TimeSpan,
            Forever
        }

        private readonly DurationType _durationType;
        private readonly TimeSpan _timeSpan;

        /// <summary>
        ///     Flag, specifies does this duration has explicit <see cref="System.TimeSpan" /> value, defined as duration
        /// </summary>
        public bool HasTimeSpan => _durationType == DurationType.TimeSpan;

        /// <summary>
        ///     Explicit <see cref="System.TimeSpan" /> value, defined as duration
        /// </summary>
        /// <exception cref="InvalidOperationException">If duration hasn't explicit TimeSpan defined</exception>
        public TimeSpan TimeSpan
        {
            get
            {
                if (HasTimeSpan)
                    return _timeSpan;
                throw new InvalidOperationException("Not explicit TimeSpan duration");
            }
        }

        /// <summary>
        ///     Constructor, which allow to setup duration with <see cref="System.TimeSpan" /> value
        /// </summary>
        /// <param name="timeSpan">Explicit <see cref="System.TimeSpan" /> value, defined as duration</param>
        public Duration(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
            _durationType = DurationType.TimeSpan;
        }

        private Duration(DurationType type)
        {
            _durationType = type;
        }

        /// <summary>
        ///     Automatically calculated duration
        /// </summary>
        public static readonly Duration Automatic = new Duration(DurationType.Auto);

        /// <summary>
        ///     Forever execution duration
        /// </summary>
        public static readonly Duration Forever = new Duration(DurationType.Forever);

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
        {
            if (HasTimeSpan)
                // ReSharper disable once ImpureMethodCallOnReadonlyValueField
                return _timeSpan.ToString();
            return _durationType == DurationType.Forever ? "Forever" : "Automatic";
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        ///     true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value;
        ///     otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Duration d)
                return Equals(d);
            return base.Equals(obj);
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="other">The object to compare with the current instance.</param>
        /// <returns>
        ///     true if <paramref name="other">obj</paramref> and this instance are the same type and represent the same
        ///     value; otherwise, false.
        /// </returns>
        public bool Equals(Duration other)
        {
            return _durationType == other._durationType &&
                   (_durationType != DurationType.TimeSpan || _timeSpan == other._timeSpan);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) _durationType * 397) ^ _timeSpan.GetHashCode();
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool operator ==(Duration d1, Duration d2)
        {
            return d1.Equals(d2);
        }

        public static bool operator !=(Duration d1, Duration d2)
        {
            return !(d1 == d2);
        }

        public static Duration operator +(TimeSpan timeSpan, Duration duration)
        {
            if (duration.HasTimeSpan)
                return new Duration(timeSpan + duration.TimeSpan);

            return duration;
        }

        public static Duration operator +(Duration duration, TimeSpan timeSpan)
        {
            if (duration.HasTimeSpan)
                return new Duration(timeSpan + duration.TimeSpan);

            return duration;
        }

        public static Duration operator +(Duration duration1, Duration duration2)
        {
            if (duration1.HasTimeSpan && duration2.HasTimeSpan)
                return new Duration(duration1.TimeSpan + duration2.TimeSpan);
            if (duration1 == Forever || duration2 == Forever)
                return Forever;

            return Automatic;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        
        /// <summary>
        /// Parses input plain string value into <see cref="Duration"/>.
        /// </summary>
        /// <param name="value">Input string value. Permitted values are: Automatic, Forever, {TimeSpan}.</param>
        /// <returns><see cref="Duration"/> or <see cref="InvalidOperationException"/> in case input string have an invaslid format.</returns>
        public static Duration Parse(string value)
        {
            if (string.Compare(value, "Automatic", StringComparison.OrdinalIgnoreCase) == 0)
                return Automatic;
            if (string.Compare(value, "Forever", StringComparison.OrdinalIgnoreCase) == 0)
                return Forever;

            if (TimeSpan.TryParse(value, out var timeSpan))
                return new Duration(timeSpan);

            throw new InvalidOperationException($"Impossible to parse string \"{value ?? ""}\" to Duration, possible values are: Automatic, Forever or timespan value");
        }

        /// <summary>
        /// Safety parses plain string value into <see cref="Duration"/>
        /// </summary>
        /// <param name="value">Input string value. Permitted values are: Automatic, Forever, {TimeSpan}.</param>
        /// <param name="duration">resulting value in case if return value is true.</param>
        /// <returns>true if parse operation successfull, false is parse exception was occured.</returns>
        public static bool TryParse(string value, out Duration duration)
        {
            try
            {
                duration = Parse(value);
                return true;
            }
            catch (InvalidOperationException)
            {
                duration = default(Duration);
                return false;
            }
        }
    }
}