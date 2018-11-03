using System.Globalization;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values to boolean values using extended conversion rules:
    /// - Numbers: above 0, less more 0 are true; equal to 0 are false
    /// - Strings: "true", "yes", "T", "Y", "1" are true, "false", "no", "F", "N" are false
    /// - DateTime: above 0, less more 0 total milliseconds are true, equal to 0 are false
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = BooleanConverter.ToNullableBoolean(true); // true
    /// var value2 = BooleanConverter.ToNullableBoolean("yes"); // true
    /// var value3 = BooleanConverter.ToNullableBoolean(1); // true
    /// var value4 = BooleanConverter.ToNullableBoolean({}); // null
    /// </code>
    /// </example>
    public class BooleanConverter
    {
        /// <summary>
        /// Converts value into boolean or returns null when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <returns>boolean value or null when conversion is not supported.</returns>
        public static bool? ToNullableBoolean(object value)
        {
            if (value == null) return null;
            if (value is bool || value is bool?) return (bool)value;

            var strValue = System.Convert.ToString(value, CultureInfo.InvariantCulture).ToLowerInvariant();
            if (strValue == "1" || strValue == "true" || strValue == "t"
                || strValue == "yes" || strValue == "y")
                return true;

            if (strValue == "0" || strValue == "false" || strValue == "f"
                || strValue == "no" || strValue == "n")
                return false;

            return null;
        }

        /// <summary>
        /// Converts value into boolean or returns false when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <returns>boolean value or false when conversion is not supported.</returns>
        /// See <see cref="BooleanConverter.ToNullableBoolean(object)"/>
        public static bool ToBoolean(object value)
        {
            return ToBooleanWithDefault(value, false);
        }

        /// <summary>
        /// Converts value into boolean or returns default value when conversion is not possible
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>boolean value or default when conversion is not supported.</returns>
        /// See <see cref="BooleanConverter.ToNullableBoolean(object)"/>
        public static bool ToBooleanWithDefault(object value, bool defaultValue)
        {
            var result = ToNullableBoolean(value);
            return result.HasValue ? result.Value : defaultValue;
        }
    }
}