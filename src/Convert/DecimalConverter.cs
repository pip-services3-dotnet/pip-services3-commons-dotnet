using System;
using System.Globalization;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Converts objects to decimals.
    /// </summary>
    public class DecimalConverter
    {
        public static decimal? ToNullableDecimal(object value)
        {
            if (value == null) return null;
            if (value is TimeSpan || value is TimeSpan?)
                return (decimal)((TimeSpan)value).TotalMilliseconds;
            if (value is DateTime || value is DateTime?)
                return ((DateTime)value).Ticks;
            if (value is bool || value is bool?)
                return (bool)value ? 1 : 0;

            try
            {
                return System.Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        public static decimal ToDecimal(object value)
        {
            return ToDecimalWithDefault(value, 0);
        }

        public static decimal ToDecimalWithDefault(object value, decimal defaultValue)
        {
            var result = ToNullableDecimal(value);
            return result ?? defaultValue;
        }
    }
}