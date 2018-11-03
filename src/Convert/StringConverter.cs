using System;
using System.Collections;
using System.Globalization;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values into strings using extended conversion rules:
    /// - Numbers: are converted with '.' as decimal point
    /// - DateTime: using ISO format
    /// - Boolean: "true" for true and "false" for false
    /// - Arrays: as comma-separated list
    /// - Other objects: using toString() method
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = StringConverter.ToString(123.456); // Result: "123.456"
    /// var value2 = StringConverter.ToString(true); // Result: "true"
    /// var value3 = StringConverter.ToString(ZonedDateTime.now()); // Result: "2018-01-01T00:00:00.00"
    /// var value4 = StringConverter.ToString(new int[]{1, 2, 3}); // Result: "1,2,3"
    /// </code>
    /// </example>
    public class StringConverter
    {
        /// <summary>
        /// Converts value into string or returns null when value is null.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>string value or null when value is null.</returns>
        public static string ToNullableString(object value)
        {
            return ToStringWithDefault(value, null);
        }

        /// <summary>
        /// Converts value into string or returns "" when value is null.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>string value or "" when value is null.</returns>
        public static string ToString(object value)
        {
            return ToStringWithDefault(value, null);
        }

        /// <summary>
        /// Converts value into string or returns default when value is null.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>string value or default when value is null.</returns>
        public static string ToStringWithDefault(object value, string defaultValue)
        {
            if (value is JValue)
                value = ((JValue) value).Value;

            if (value == null) return defaultValue;
            if (value is string) return (string)value;

            if (value is TimeSpan || value is TimeSpan?)
                value = ((TimeSpan)value).TotalMilliseconds;

            if (value is DateTime || value is DateTime?)
                return ((DateTime)value).ToString("o", CultureInfo.InvariantCulture);

            var type = value.GetType();
            if (value is IEnumerable && type != typeof(string))
            {
                var builder = new StringBuilder();
                foreach (var item in (IEnumerable)value)
                {
                    if (builder.Length > 0)
                        builder.Append(',');
                    builder.Append(ToStringWithDefault(item, ""));
                }
                return builder.ToString();
            }

            try
            {
                return System.Convert.ToString(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}