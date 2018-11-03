using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values into objects specific by TypeCodes. 
    /// For each TypeCode this class calls corresponding converter 
    /// which applies extended conversion rules to convert the values.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = TypeConverter.toType(TypeCode.Integer, "123.456"); // Result: 123
    /// var value2 = TypeConverter.toType(TypeCode.DateTime, 123); // Result: DateTime(123)
    /// var value3 = TypeConverter.toType(TypeCode.Boolean, "F"); // Result: false
    /// </code>
    /// </example>
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

        /// <summary>
        /// Gets TypeCode for specific type.
        /// </summary>
        /// <param name="type">the Class type for the data type.</param>
        /// <returns>the TypeCode that corresponds to the passed object's type.</returns>
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

        /// <summary>
        /// Gets TypeCode for specific value.
        /// </summary>
        /// <param name="value">value whose TypeCode is to be resolved.</param>
        /// <returns>the TypeCode that corresponds to the passed object's type.</returns>
        public static TypeCode ToTypeCode(object value)
        {
            if (value == null)
                return TypeCode.Unknown;

            return ToTypeCode(value.GetType());
        }

        /// <summary>
        /// Converts value into an object type specified by Type Code or returns null
        /// when conversion is not possible.
        /// </summary>
        /// <typeparam name="T">the Class type for the data type.</typeparam>
        /// <param name="value">the value to convert.</param>
        /// <returns>object value of type corresponding to TypeCode, or null when
        /// conversion is not supported.</returns>
        /// See <see cref="TypeConverter.ToTypeCode(Type)"/>
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

        /// <summary>
        /// Converts value into an object type specified by Type Code 
        /// or returns type default when conversion is not possible.
        /// </summary>
        /// <typeparam name="T">the Class type for the data type into which 'value' is to be converted.</typeparam>
        /// <param name="value">the value to convert</param>
        /// <returns>object value of type corresponding to TypeCode, or type default when
        /// conversion is not supported.</returns>
        /// See <see cref="ToTypeWithDefault{T}(object, T)"/>
        public static T ToType<T>(object value)
        {
            return ToTypeWithDefault(value, default(T));
        }

        /// <summary>
        /// Converts value into an object type specified by Type Code 
        /// or returns default value when conversion is not possible.
        /// </summary>
        /// <typeparam name="T">the Class type for the data type into which 'value' is to be converted.</typeparam>
        /// <param name="value">the value to convert.</param>
        /// <param name="defaultValue">the default value to return if conversion is not possible
        /// (returns null).</param>
        /// <returns>object value of type corresponding to TypeCode, or default value when 
        /// conversion is not supported</returns>
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

        /// <summary>
        /// Converts a TypeCode into its string name.
        /// </summary>
        /// <param name="type">the TypeCode to convert into a string.</param>
        /// <returns>the name of the TypeCode passed as a string value.</returns>
        public static String ToString(TypeCode type)
        {
            switch (type)
            {
                case TypeCode.Unknown:
                    return "unknown";
                case TypeCode.String:
                    return "string";
                case TypeCode.Boolean:
                    return "boolean";
                case TypeCode.Integer:
                    return "integer";
                case TypeCode.Long:
                    return "long";
                case TypeCode.Float:
                    return "float";
                case TypeCode.Double:
                    return "double";
                case TypeCode.DateTime:
                    return "datetime";
                case TypeCode.Object:
                    return "object";
                case TypeCode.Enum:
                    return "enum";
                case TypeCode.Array:
                    return "array";
                case TypeCode.Map:
                    return "map";
                default:
                    return "unknown";
            }
        }

    }
}