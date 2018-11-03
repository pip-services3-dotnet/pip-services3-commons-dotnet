using System;
using System.Globalization;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values into double using extended conversion rules:
    /// - Strings are converted to double values
    /// - DateTime: total number of milliseconds since unix epoсh
    /// - Boolean: 1 for true and 0 for false
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = DoubleConverter.ToNullableDouble("ABC"); // Result: null
    /// var value2 = DoubleConverter.ToNullableDouble("123.456"); // Result: 123.456
    /// var value3 = DoubleConverter.ToNullableDouble(true); // Result: 1
    /// var value4 = DoubleConverter.ToNullableDouble(new Date()); // Result: current milliseconds
    /// </code>
    /// </example>
    public class DoubleConverter
    {
        /// <summary>
        /// Converts value into doubles or returns null when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <returns>double value or null when conversion is not supported.</returns>
        public static double? ToNullableDouble(object value)
        {
            if (value == null) return null;
            if (value is TimeSpan || value is TimeSpan?)
                return (double)((TimeSpan)value).TotalMilliseconds;
            if (value is DateTime || value is DateTime?)
                return ((DateTime)value).Ticks;
            if (value is bool || value is bool?)
                return (bool)value ? 1 : 0;

            try
            {
                return System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts value into doubles or returns 0 when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <returns>double value or 0 when conversion is not supported.</returns>
        /// See <see cref="DoubleConverter.ToNullableDouble(object)"/>
        public static double ToDouble(object value)
        {
            return ToDoubleWithDefault(value, 0);
        }

        /// <summary>
        /// Converts value into doubles or returns default when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>double value or default when conversion is not supported.</returns>
        /// See <see cref="DoubleConverter.ToNullableDouble(object)"/>
        public static double ToDoubleWithDefault(object value, double defaultValue)
        {
            var result = ToNullableDouble(value);
            return result.HasValue ? result.Value : defaultValue;
        }
    }
}