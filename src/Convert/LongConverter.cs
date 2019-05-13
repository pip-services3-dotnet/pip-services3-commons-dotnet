using System;
using System.Globalization;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values into longs using extended conversion rules:
    /// - Strings are converted to floats, then to longs
    /// - DateTime: total number of milliseconds since unix epoсh
    /// - Boolean: 1 for true and 0 for false
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = LongConverter.ToNullableLong("ABC"); // Result: null
    /// var value2 = LongConverter.ToNullableLong("123.456"); // Result: 123
    /// var value3 = LongConverter.ToNullableLong(true); // Result: 1
    /// var value4 = LongConverter.ToNullableLong(new Date()); // Result: current milliseconds
    /// </code>
    /// </example>
    public class LongConverter
    {
        /// <summary>
        /// Converts value into long or returns null when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>long value or null when conversion is not supported.</returns>
        public static long? ToNullableLong(object value)
        {
            if (value == null) return null;
            if (value is TimeSpan || value is TimeSpan?)
                return (long)((TimeSpan)value).Ticks;
            if (value is DateTime || value is DateTime?)
                return ((DateTime)value).Ticks;
            if (value is bool || value is bool?)
                return (bool)value ? 1 : 0;

            try
            {
                if (value is string && (value as string).Contains("."))
                {
                    return (long)System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
                }

                return System.Convert.ToInt64(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts value into long or returns 0 when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>long value or 0 when conversion is not supported.</returns>
        public static long ToLong(object value)
        {
            return ToLongWithDefault(value, 0);
        }

        /// <summary>
        /// Converts value into long or returns default when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <param name="defaultValue">the default value.</param>
        /// <returns>long value or default when conversion is not supported.</returns>
        public static long ToLongWithDefault(object value, long defaultValue)
        {
            var result = ToNullableLong(value);
            return result.HasValue ? result.Value : defaultValue;
        }
    }
}