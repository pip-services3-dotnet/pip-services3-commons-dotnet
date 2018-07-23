using System;
using System.Globalization;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Converts objects to integers.
    /// </summary>
    public class IntegerConverter
    {
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
                return (int)System.Convert.ToSingle(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        public static int ToInteger(object value)
        {
            return ToIntegerWithDefault(value, 0);
        }

        public static int ToIntegerWithDefault(object value, int defaultValue)
        {
            var result = ToNullableInteger(value);
            return result.HasValue ? result.Value : defaultValue;
        }
    }
}