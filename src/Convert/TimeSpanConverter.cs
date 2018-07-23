using System;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Converts objects to TimeSpans.
    /// </summary>
    public class TimeSpanConverter
    {
        public static TimeSpan? ToNullableTimeSpan(object value)
        {
            if (value == null) return null;
            if (value is DateTime || value is DateTime?) return null;
            if (value is TimeSpan || value is TimeSpan?) return (TimeSpan)value;
            if (value is int) return TimeSpan.FromMilliseconds((int)value);
            if (value is long) return TimeSpan.FromMilliseconds((long)value);
            if (value is float) return TimeSpan.FromMilliseconds((float)value);
            if (value is double) return TimeSpan.FromMilliseconds((double)value);

            try
            {
                float? millis = FloatConverter.ToNullableFloat(value);
                if (millis.HasValue) return TimeSpan.FromMilliseconds(millis.Value);
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static TimeSpan ToTimeSpan(object value)
        {
            return ToTimeSpanWithDefault(value, new TimeSpan(0));
        }

        public static TimeSpan ToTimeSpanWithDefault(object value, TimeSpan? defaultValue)
        {
            var realDefault = defaultValue.HasValue ? defaultValue.Value : new TimeSpan(0);
            var result = ToNullableTimeSpan(value);
            return result.HasValue ? result.Value : realDefault;
        }
    }
}