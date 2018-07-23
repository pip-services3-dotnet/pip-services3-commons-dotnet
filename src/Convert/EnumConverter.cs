using System;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Converts objects to Enums.
    /// </summary>
    public class EnumConverter
    {
        public static T? ToNullableEnum<T>(object value) where T : struct
        {
            if (value == null) return null;

            try
            {
                return (T)value;
            }
            catch
            {
                try
                {
                    // Todo: Try to avoid exception
                    return (T)Enum.Parse(typeof(T), value.ToString());
                }
                catch
                {
                    return null;
                }
            }
        }

        public static T ToEnum<T>(object value)
        {
            return ToEnumWithDefault<T>(value, default(T));
        }

        public static T ToEnumWithDefault<T>(object value, T defaultValue)
        {
            if (value == null) return defaultValue;

            try
            {
                return (T)value;
            }
            catch
            {
                try
                {
                    // Todo: try to avoid exception
                    return (T)Enum.Parse(typeof(T), value.ToString());
                }
                catch
                {
                    return defaultValue;
                }
            }
        }
    }
}