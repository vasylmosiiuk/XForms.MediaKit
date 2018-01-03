using System;
using Xamarin.Forms;

namespace MediaKit.Core.TypeConverters
{
    public class DurationTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFromInvariantString(string value)
        {
            if (string.Compare(value, "Automatic", StringComparison.OrdinalIgnoreCase) == 0)
                return Duration.Automatic;
            if (string.Compare(value, "Forever", StringComparison.OrdinalIgnoreCase) == 0)
                return Duration.Forever;

            if (TimeSpan.TryParse(value, out var timeSpan))
                return new Duration(timeSpan);

            throw new InvalidOperationException($"Impossible to parse string \"{value??""}\" to Duration, possible values are: Automatic, Forever or timespan value");
        }
    }
}