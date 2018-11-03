using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using PipServices3.Commons.Convert;

namespace PipServices3.Commons.Reflect
{
    /// <summary>
    /// Helper class matches value types for equality.
    /// This class has symmetric implementation across all languages supported
    /// by Pip.Services toolkit and used to support dynamic data processing.
    /// </summary>
    /// See <see cref="Convert.TypeCode"/>
    public static class TypeMatcher
    {
        /// <summary>
        /// Matches expected type to a type of a value. The expected type can be 
        /// specified by a type, type name or TypeCode.
        /// </summary>
        /// <param name="expectedType">an expected type to match.</param>
        /// <param name="actualValue">a value to match its type to the expected one.</param>
        /// <returns>true if types are matching and false if they don't.</returns>
        /// See <see cref="MatchType(object, Type)"/>
        public static bool MatchValue(object expectedType, object actualValue)
        {
            if (expectedType == null)
                return true;
            if (actualValue == null)
                throw new ArgumentNullException(nameof(actualValue), "Actual value cannot be null");

            return MatchType(expectedType, actualValue.GetType());
        }

        /// <summary>
        /// Matches expected type to a type of a value.
        /// </summary>
        /// <param name="expectedType">an expected type to match.</param>
        /// <param name="actualValue">a value to match its type to the expected one.</param>
        /// <returns>true if types are matching and false if they don't.</returns>
        public static bool MatchValueByName(string expectedType, object actualValue)
        {
            if (expectedType == null)
                return true;
            if (actualValue == null)
                throw new ArgumentNullException(nameof(actualValue), "Actual value cannot be null");

            return MatchTypeByName(expectedType, actualValue.GetType());
        }

        /// <summary>
        /// Matches expected type to an actual type. The types can be specified as types,
        /// type names or TypeCode.
        /// </summary>
        /// <param name="expectedType">an expected type to match.</param>
        /// <param name="actualType">an actual type to match.</param>
        /// <returns>true if types are matching and false if they don't.</returns>
        /// See <see cref="MatchTypeByName(string, Type)"/>
        public static bool MatchType(object expectedType, Type actualType)
        {
            if (expectedType == null)
                return true;
            if (actualType == null)
                throw new ArgumentNullException(nameof(actualType), "Actual type cannot be null");

            var type = expectedType as Type;
            if (type != null)
                return type.GetTypeInfo().IsAssignableFrom(actualType);

            if (expectedType.Equals(actualType))
                return true;

            var str = expectedType as string;
            if (str != null)
                return MatchTypeByName(str, actualType);

            if (expectedType is Convert.TypeCode)
                return TypeConverter.ToTypeCode(actualType).Equals(expectedType);

            return false;
        }

        /// <summary>
        /// Matches expected type to an actual type.
        /// </summary>
        /// <param name="expectedType">an expected type name to match.</param>
        /// <param name="actualType">an actual type to match defined by type code.</param>
        /// <returns>true if types are matching and false if they don't.</returns>
        public static bool MatchTypeByName(string expectedType, Type actualType)
        {
            if (expectedType == null)
                return true;

            if (actualType == null)
                throw new ArgumentNullException(nameof(actualType), "Actual type cannot be null");

            expectedType = expectedType.ToLower();

            if (actualType.Name.Equals(expectedType, StringComparison.OrdinalIgnoreCase))
                return true;

            if (expectedType.Equals("object"))
                return true;

            if (expectedType.Equals("int") || expectedType.Equals("integer"))
                return actualType == typeof(int)
                    || actualType == typeof(int?)
                    || actualType == typeof(long)
                    || actualType == typeof(long?);

            if (expectedType.Equals("long"))
                return actualType == typeof(int)
                    || actualType == typeof(int?)
                    || actualType == typeof(long)
                    || actualType == typeof(long?);

            if (expectedType.Equals("float"))
                return actualType == typeof(float) 
                    || actualType == typeof(float?)
                    || actualType == typeof(double)
                    || actualType == typeof(double?)
                    || actualType == typeof(decimal)
                    || actualType == typeof(decimal?);

            if (expectedType.Equals("double"))
                return actualType == typeof(double)
                    || actualType == typeof(double?)
                    || actualType == typeof(decimal)
                    || actualType == typeof(decimal?);

            if (expectedType.Equals("string"))
                return actualType == typeof(string);

            if (expectedType.Equals("bool") || expectedType.Equals("boolean"))
                return actualType == typeof(bool)
                    || actualType == typeof(bool);

            if (expectedType.Equals("date") || expectedType.Equals("datetime"))
                return actualType == typeof(DateTime) 
                    || actualType == typeof(DateTime?)
                    || actualType == typeof(DateTimeOffset)
                    || actualType == typeof(DateTimeOffset?);

            if (expectedType.Equals("timespan") || expectedType.Equals("duration"))
                return actualType == typeof(TimeSpan)
                    || actualType == typeof(TimeSpan?)
                    || actualType == typeof(int)
                    || actualType == typeof(int?)
                    || actualType == typeof(float)
                    || actualType == typeof(float?)
                    || actualType == typeof(double)
                    || actualType == typeof(double?);

            if (expectedType.Equals("enum"))
                return actualType.GetTypeInfo().IsEnum;

            if (expectedType.Equals("map") || expectedType.Equals("dict") || expectedType.Equals("dictionary"))
            {
                var type = actualType.GetTypeInfo();
                return type.GetInterfaces().Contains(typeof(IDictionary))
                    || type.GetInterfaces().Contains(typeof(IDictionary<,>))
#if !CORE_NET
                    || typeof(IDictionary).IsAssignableFrom(actualType)
#endif
                    ;
            }

            if (expectedType.Equals("array") || expectedType.Equals("list"))
            {
                var type = actualType.GetTypeInfo();
                return actualType.IsArray
                       || type.GetInterfaces().Contains(typeof(IList))
                       || type.GetInterfaces().Contains(typeof(IList<>))
#if !CORE_NET
                       || typeof(IEnumerable).IsAssignableFrom(actualType)
#endif
                       ;
            }

            if (expectedType.EndsWith("[]"))
            {
                // Todo: Check subtype
                var type = actualType.GetTypeInfo();
                return actualType.IsArray
                       || type.GetInterfaces().Contains(typeof(IList))
                       || type.GetInterfaces().Contains(typeof(IList<>));
            }

            return false;
        }

        public static bool MatchEnum(object expectedType, object value)
        {
            if (value == null)
                return false;

            if (!(expectedType is Type))
                return false;

            var type = expectedType as Type;
#if !CORE_NET
            if (type.IsEnum == false)
                return false;
#endif

            try
            {
                int? intValue = IntegerConverter.ToNullableInteger(value);
                if (intValue != null && Enum.ToObject(type, intValue) != null)
                    return true;
            }
            catch
            {
                // Ignore...
            }

            try
            {
                string strValue = StringConverter.ToNullableString(value);
                if (strValue != null && Enum.Parse(type, strValue) != null)
                    return true;
            }
            catch
            {
                // Ignore...
            }

            return false;
        }
    }
}
