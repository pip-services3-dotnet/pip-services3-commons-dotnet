using System;
using System.Globalization;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Converts objects to longs.
    /// </summary>
    public class LongConverter
    {
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
                return (long)System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        public static long ToLong(object value)
        {
            return ToLongWithDefault(value, 0);
        }

        public static long ToLongWithDefault(object value, long defaultValue)
        {
            var result = ToNullableLong(value);
            return result.HasValue ? result.Value : defaultValue;
        }
    }
}