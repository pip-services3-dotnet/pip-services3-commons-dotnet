using System;
using System.Globalization;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Converts objects to floats.
    /// </summary>
    public class FloatConverter
    {
        public static float? ToNullableFloat(object value)
        {
            if (value == null) return null;
            if (value is TimeSpan || value is TimeSpan?)
                return (float)((TimeSpan)value).TotalMilliseconds;
            if (value is DateTime || value is DateTime?)
                return ((DateTime)value).Ticks;
            if (value is bool || value is bool?)
                return (bool)value ? 1 : 0;

            try
            {
                return System.Convert.ToSingle(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        public static float ToFloat(object value)
        {
            return ToFloatWithDefault(value, 0);
        }

        public static float ToFloatWithDefault(object value, float defaultValue)
        {
            var result = ToNullableFloat(value);
            return result.HasValue ? result.Value : defaultValue;
        }
    }
}