using System;
using Xamarin.Forms;

namespace MediaKit.Core.TypeConverters
{
    /// <summary>
    /// XAML type converter for <see cref="RepeatBehavior"/>
    /// </summary>
    public class RepeatBehaviorTypeConverter : TypeConverter
    {
        /// <param name="sourceType">The type to check.</param>
        /// <summary>When implemented in a derived class, returns a Boolean value that indicates whether or not the derived type converter can convert <paramref name="sourceType" /> to its target type.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public override bool CanConvertFrom(Type sourceType) => sourceType == typeof(string);

        /// <param name="value">The value to convert.</param>
        /// <summary>When overriden in a derived class, converts XAML extension syntax into instances of various <see cref="N:Xamarin.Forms" /> types.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public override object ConvertFromInvariantString(string value) => RepeatBehavior.Parse(value);
    }
}