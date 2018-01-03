using System;
using Xamarin.Forms;

namespace MediaKit.Core.TypeConverters
{
    public class RepeatBehaviorTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(Type sourceType) => sourceType == typeof(string);

        public override object ConvertFromInvariantString(string value)
        {
            value = value?.Trim() ?? "";
            if (string.IsNullOrEmpty(value))
                return new RepeatBehavior();
            if (string.Compare(value, "Forever", StringComparison.OrdinalIgnoreCase) == 0)
                return RepeatBehavior.Forever;
            if (value.Length > 1 && value.EndsWith("x") &&
                uint.TryParse(value.Substring(0, value.Length - 1), out var count))
                return new RepeatBehavior(count);
            if (TimeSpan.TryParse(value, out var timeSpan))
                return new RepeatBehavior(timeSpan);

            throw new InvalidOperationException(
                $"Impossible to parse string \"{value}\" to RepeatBehavior, possible values are: Forever, timespan or %Count%x values");
        }
    }
}