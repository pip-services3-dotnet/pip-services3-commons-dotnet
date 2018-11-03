using System;
using System.Globalization;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values into Date values using extended conversion rules:
    /// - Strings: converted using ISO time format
    /// - Numbers: converted using milliseconds since unix epoch
    /// </summary>
    /// <example>
    /// <code>
    /// DateTime value1 = DateTimeConverter.ToNullableDateTime("ABC"); // Result: null
    /// DateTime value2 = DateTimeConverter.ToNullableDateTime("2018-01-01T11:30:00.0"); // Result: ZonedDateTime(2018,0,1,11,30)
    /// DateTime value3 = DateTimeConverter.ToNullableDateTime(123); // Result: ZonedDateTime(123)
    /// </code>
    /// </example>
    public class DateTimeConverter
    {
        /// <summary>
        /// Converts value into Date or returns null when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <returns>Date value or null when conversion is not supported.</returns>
        public static DateTime? ToNullableDateTime(object value)
        {
            if (value == null) return null;
            if (value is DateTime || value is DateTime?)
                return (DateTime)value;
            if (value is TimeSpan || value is TimeSpan?)
                value = (long)((TimeSpan)value).TotalMilliseconds;
            if (value is int) return new DateTime((int)value);
            if (value is long) return new DateTime((long)value);

            try
            {
                if (value is string)
                    return DateTime.Parse((string)value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

                return System.Convert.ToDateTime(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts value into Date or returns current when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <returns>Date value or current when conversion is not supported.</returns>
        /// See <see cref="DateTimeConverter.ToNullableDateTime(object)"/>
        public static DateTime ToDateTime(object value)
        {
            return ToDateTimeWithDefault(value, new DateTime());
        }

        /// <summary>
        /// Converts value into Date or returns default when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>Date value or default when conversion is not supported.</returns>
        /// See <see cref="DateTimeConverter.ToNullableDateTime(object)"/>
        public static DateTime ToDateTimeWithDefault(object value, DateTime? defaultValue)
        {
            var realDefault = defaultValue ?? new DateTime();
            var result = ToNullableDateTime(value);
            return result ?? realDefault;
        }
    }
}