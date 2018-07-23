using System;
using System.Globalization;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Converts objects to doubles.
    /// </summary>
    public class DoubleConverter
    {
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

        public static double ToDouble(object value)
        {
            return ToDoubleWithDefault(value, 0);
        }

        public static double ToDoubleWithDefault(object value, double defaultValue)
        {
            var result = ToNullableDouble(value);
            return result.HasValue ? result.Value : defaultValue;
        }
    }
}