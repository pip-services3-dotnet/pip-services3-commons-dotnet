using System;
using System.Collections;
using System.Globalization;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Converts objects to string.
    /// </summary>
    public class StringConverter
    {
        public static string ToNullableString(object value)
        {
            return ToStringWithDefault(value, null);
        }

        public static string ToString(object value)
        {
            return ToStringWithDefault(value, null);
        }

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