using System;
using System.Globalization;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values into integer using extended conversion rules:
    /// - Strings are converted to integer values
    /// - DateTime: total number of milliseconds since unix epoсh
    /// - Boolean: 1 for true and 0 for false
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = IntegerConverter.ToNullableInteger("ABC"); // Result: null
    /// var value2 = IntegerConverter.ToNullableInteger("123.456"); // Result: 123.456
    /// var value3 = IntegerConverter.ToNullableInteger(true); // Result: 1
    /// var value4 = IntegerConverter.ToNullableInteger(new Date()); // Result: current milliseconds
    /// </code>
    /// </example>
    public class IntegerConverter
    {
        /// <summary>
        /// Converts value into integer or returns null when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>integer value or null when conversion is not supported.</returns>
        public static int? ToNullableInteger(object value)
        {
            if (value == null) return null;
            if (value is DateTime || value is DateTime?)
                return (int)((DateTime)value).Ticks;
            if (value is TimeSpan || value is TimeSpan?)
                return (int)((TimeSpan)value).Ticks;
            if (value is bool || value is bool?)
                return (bool)value ? 1 : 0;

            try
            {
                if (value is string && (value as string).Contains("."))
                {
                    return (int)System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
                }

                return System.Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts value into integer or returns 0 when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>integer value or 0 when conversion is not supported.</returns>
        public static int ToInteger(object value)
        {
            return ToIntegerWithDefault(value, 0);
        }

        /// <summary>
        /// Converts value into integer or returns default when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>integer value or default when conversion is not supported.</returns>
        public static int ToIntegerWithDefault(object value, int defaultValue)
        {
            var result = ToNullableInteger(value);
            return result.HasValue ? result.Value : defaultValue;
        }
    }
}