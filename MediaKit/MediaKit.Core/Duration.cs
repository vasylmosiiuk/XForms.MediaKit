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
        
        /// <summary>
        /// <see cref="object.ToString"/> implementation
        /// </summary>
        /// <returns>String representation this <see cref="Duration"/> object</returns>
        public override string ToString()
        {
            if (HasTimeSpan)
                // ReSharper disable once ImpureMethodCallOnReadonlyValueField
                return _timeSpan.ToString();
            return _durationType == DurationType.Forever ? "Forever" : "Automatic";
        }

        public override bool Equals(object obj)
        {
            if (obj is Duration d)
                return Equals(d);
            return base.Equals(obj);
        }

        public bool Equals(Duration other)
        {
            return _durationType == other._durationType &&
                   (_durationType != DurationType.TimeSpan || _timeSpan == other._timeSpan);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) _durationType * 397) ^ _timeSpan.GetHashCode();
            }
        }

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
    }
}