using System;
using System.Globalization;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values into float  using extended conversion rules:
    /// - Strings are converted to float values
    /// - DateTime: total number of milliseconds since unix epoсh
    /// - Boolean: 1 for true and 0 for false
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = FloatConverter.ToNullableFloat("ABC"); // Result: null
    /// var value2 = FloatConverter.ToNullableFloat("123.456"); // Result: 123.456
    /// var value3 = FloatConverter.ToNullableFloat(true); // Result: 1
    /// var value4 = FloatConverter.ToNullableFloat(new Date()); // Result: current milliseconds
    /// </code>
    /// </example>
    public class FloatConverter
    {
        /// <summary>
        /// Converts value into float or returns null when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <returns>float value or null when conversion is not supported.</returns>
        public static float? ToNullableFloat(object value)
        {
            if (value == null) return null;
            if (value is TimeSpan || value is TimeSpan?)
                return (float)((TimeSpan)value).TotalMilliseconds;
            if (value is DateTime || value is DateTime?)
                return ((DateTime)value).Ticks;
            if (value is bool || value is bool?)
                return (bool)value ? 1 : 0;

            try
            {
                return System.Convert.ToSingle(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts value into float or returns 0 when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <returns>float value or 0 when conversion is not supported.</returns>
        public static float ToFloat(object value)
        {
            return ToFloatWithDefault(value, 0);
        }

        /// <summary>
        /// Converts value into float or returns default when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>float value or default when conversion is not supported.</returns>
        public static float ToFloatWithDefault(object value, float defaultValue)
        {
            var result = ToNullableFloat(value);
            return result.HasValue ? result.Value : defaultValue;
        }
    }
}