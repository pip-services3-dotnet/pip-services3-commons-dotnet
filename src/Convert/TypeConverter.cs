using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Generic type converter.
    /// </summary>
    public static class TypeConverter
    {
        public static bool IsPrimitiveType(object obj)
        {
            var type = obj.GetType();

            return IsPrimitiveType(type);
        }

        public static bool IsPrimitiveType(Type type)
        {
            return type == typeof(string) || type == typeof(int) || type == typeof(long) || type == typeof(decimal) 
                || type == typeof(char) || type == typeof(decimal) || type == typeof(bool) || type == typeof(byte) 
                || type == typeof(double) || type == typeof(float) || type == typeof(sbyte) || type == typeof(short) 
                || type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort);
        }

        public static TypeCode ToTypeCode(Type type)
        {
            if (type == null)
                return TypeCode.Unknown;

            if (type.IsArray)
                return TypeCode.Array;

            if (type.GetTypeInfo().IsEnum)
                return TypeCode.Enum;

            if (type == typeof(bool))
                return TypeCode.Boolean;

            if (type == typeof(double))
                return TypeCode.Double;

            if (type == typeof(float))
                return TypeCode.Float;

            if (type == typeof(long))
                return TypeCode.Long;

            if (type == typeof(int))
                return TypeCode.Integer;

            if (type == typeof(string))
                return TypeCode.String;

            if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
                return TypeCode.DateTime;

            if (type == typeof(TimeSpan))
                return TypeCode.Duration;

            if (type.GetTypeInfo().GetInterface(nameof(IDictionary)) != null 
                || type.GetTypeInfo().GetInterfaces().Contains(typeof(IDictionary<,>)))
                return TypeCode.Map;

            if (type.GetTypeInfo().GetInterface(nameof(IList)) != null
                || type.GetTypeInfo().GetInterfaces().Contains(typeof(IList<>)))
                return TypeCode.Array;

            return TypeCode.Object;
        }

        public static TypeCode ToTypeCode(object value)
        {
            if (value == null)
                return TypeCode.Unknown;

            return ToTypeCode(value.GetType());
        }

        public static T? ToNullableType<T>(object value) where T : struct
        {
            if (value == null) return null;
            if (value is T) return (T)value;

            var typeInfo = typeof(T).GetTypeInfo();

            if (typeInfo.IsEnum)
                value = EnumConverter.ToNullableEnum<T>(value);
            else if (typeInfo.IsAssignableFrom(typeof(decimal)))
                value = DecimalConverter.ToNullableDecimal(value);
            else if (typeInfo.IsAssignableFrom(typeof(string)))
                value = StringConverter.ToNullableString(value);
            else if (typeInfo.IsAssignableFrom(typeof(long)))
                value = LongConverter.ToNullableLong(value);
            else if (typeInfo.IsAssignableFrom(typeof(int)))
                value = IntegerConverter.ToNullableInteger(value);
            else if (typeInfo.IsAssignableFrom(typeof(double)))
                value = DoubleConverter.ToNullableDouble(value);
            else if (typeInfo.IsAssignableFrom(typeof(float)))
                value = FloatConverter.ToNullableFloat(value);
            else if (typeInfo.IsAssignableFrom(typeof(DateTime)))
                value = DateTimeConverter.ToNullableDateTime(value);
            else if (typeInfo.IsAssignableFrom(typeof(TimeSpan)))
                value = TimeSpanConverter.ToNullableTimeSpan(value);

            if (value == null) return null;

            try
            {
                return (T)value;
            }
            catch
            {
                return null;
            }
        }

        public static T ToType<T>(object value)
        {
            return ToTypeWithDefault(value, default(T));
        }

        public static T ToTypeWithDefault<T>(object value, T defaultValue)
        {
            if (value == null) return defaultValue;
            if (value is T) return (T)value;

            var typeInfo = typeof(T).GetTypeInfo();

            if (typeInfo.IsEnum)
                value = EnumConverter.ToEnumWithDefault<T>(value, defaultValue);
            else if (typeInfo.IsAssignableFrom(typeof(decimal)))
                value = DecimalConverter.ToNullableDecimal(value);
            else if (typeInfo.IsAssignableFrom(typeof(string)))
                value = StringConverter.ToNullableString(value);
            else if (typeInfo.IsAssignableFrom(typeof(long)))
                value = LongConverter.ToNullableLong(value);
            else if (typeInfo.IsAssignableFrom(typeof(int)))
                value = IntegerConverter.ToNullableInteger(value);
            else if (typeInfo.IsAssignableFrom(typeof(double)))
                value = DoubleConverter.ToNullableDouble(value);
            else if (typeInfo.IsAssignableFrom(typeof(float)))
                value = FloatConverter.ToNullableFloat(value);
            else if (typeInfo.IsAssignableFrom(typeof(bool)))
                value = BooleanConverter.ToNullableBoolean(value);
            else if (typeInfo.IsAssignableFrom(typeof(DateTime)))
                value = DateTimeConverter.ToNullableDateTime(value);
            else if (typeInfo.IsAssignableFrom(typeof(TimeSpan)))
                value = TimeSpanConverter.ToNullableTimeSpan(value);

            if (value == null) return defaultValue;

            try
            {
                if (typeInfo.IsClass || typeInfo.IsInterface)
                    return value is T ? (T)value : defaultValue;
                else
                    return (T)value;
            }
            catch
            {
                return defaultValue;
            }
        }

    }
}