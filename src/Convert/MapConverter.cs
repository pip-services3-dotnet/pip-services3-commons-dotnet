using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values into map objects using extended conversion rules:
    /// - Objects: property names as keys, property values as values
    /// - Arrays: element indexes as keys, elements as values
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = MapConverted.ToNullableMap("ABC"); // Result: null
    /// var value2 = MapConverted.ToNullableMap({ key: 123 }); // Result: { key: 123 }
    /// var value3 = MapConverted.ToNullableMap(new int[] { 1, 2, 3 }); // Result: { "0": 1, "1": 2, "2": 3 }
    /// </code>
    /// </example>
    public class MapConverter
    {
        private static IDictionary<string, object> MapToMap(IDictionary dictionary)
        {
            var result = new Dictionary<string, object>();
            foreach(var key in dictionary.Keys)
                result[StringConverter.ToString(key)] = dictionary[key];
            return result;
        }

        private static IDictionary<string, object> ArrayToMap(IEnumerable enumerable)
        {
            var result = new Dictionary<string, object>();
            var index = 0;
            foreach (var obj in enumerable)
                result[(index++).ToString()] = obj;
            return result;
        }

        /// <summary>
        /// Converts value into map object or returns null when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>map object or null when conversion is not supported.</returns>
        public static IDictionary<string, object> ToNullableMap(object value)
        {
            if (value == null) return null;

            var type = value.GetType();
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsPrimitive) return null;

            var dictionary = value as IDictionary;
            if(dictionary != null)
                return MapToMap(dictionary);

            var enumerable = value as IEnumerable;
            if (enumerable != null && !(value is string) && !(value is JObject || value is JArray))
                return ArrayToMap(enumerable);

            try
            {
                return RecursiveMapConverter.ToNullableMap(value);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts value into map object or returns empty map when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>map object or empty map when conversion is not supported.</returns>
        public static IDictionary<string, object> ToMap(object value)
        {
            var result = ToNullableMap(value);
            return result ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Converts value into map object or returns default map when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <param name="defaultValue">the default value.</param>
        /// <returns>map object or default map when conversion is not supported.</returns>
        public static IDictionary<string, object> ToMapWithDefault(
            object value, Dictionary<string, object> defaultValue)
        {
            var result = ToNullableMap(value);
            return result ?? defaultValue;
        }
    }
}