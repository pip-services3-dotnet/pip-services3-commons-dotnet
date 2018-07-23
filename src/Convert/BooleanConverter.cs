using System.Globalization;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Converts objects to booleans.
    /// </summary>
    public class BooleanConverter
    {
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

        public static bool ToBoolean(object value)
        {
            return ToBooleanWithDefault(value, false);
        }

        public static bool ToBooleanWithDefault(object value, bool defaultValue)
        {
            var result = ToNullableBoolean(value);
            return result.HasValue ? result.Value : defaultValue;
        }
    }
}