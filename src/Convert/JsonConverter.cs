using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values from and to JSON (JavaScript Object Notation) strings.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = JsonConverter.fromJson("{\"key\":123}"); // Result: { key: 123 }
    /// var value2 = JsonConverter.ToMap({ key: 123}); // Result: "{\"key\":123}"
    /// </code>
    /// </example>
    public static class JsonConverter
    {
        private static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<Newtonsoft.Json.JsonConverter>
            {
                new Newtonsoft.Json.Converters.StringEnumConverter()
            }
        };

        /// <summary>
        /// Converts value into JSON string.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns> JSON string or null when value is null.</returns>
        public static string ToJson(object value)
        {
            if (value == null) return null;
            return JsonConvert.SerializeObject(
                value,
                Formatting.None, 
                JsonSettings
            );
        }

        /// <summary>
        /// Converts value from JSON string
        /// </summary>
        /// <param name="value">the JSON string to convert.</param>
        /// <returns>converted object value or null when value is null.</returns>
        public static object FromJson(string value)
        {
            if (value == null) return null;
            return JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// Converts value from JSON string to T object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">the JSON string to convert.</param>
        /// <returns>converted object value or null when value is null.</returns>
        public static T FromJson<T>(string value)
        {
            if (value == null) return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Converts JSON string into map object or returns null when conversion is not possible.
        /// </summary>
        /// <param name="value">the JSON string to convert.</param>
        /// <returns>Map object value or null when conversion is not supported.</returns>
        public static IDictionary<string, object> ToNullableMap(string value)
        {
            if (value == null) return null;

            try
            {
                var map = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                    value, new JsonSerializerSettings());

                ConvertJsonTypes(map);
                return map;

                //return RecursiveMapConverter.ToNullableMap(map);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ConvertJsonTypes(IDictionary<string, object> dict)
        {
            foreach (var pair in dict.ToArray())
            {
                var jObject = pair.Value as JObject;
                if (jObject != null)
                {
                    var newDict = jObject.ToObject<Dictionary<string, object>>();

                    dict[pair.Key] = newDict;

                    ConvertJsonTypes(newDict);
                }

                var jArray = pair.Value as JArray;
                if (jArray != null)
                {
                    var newList = jArray.ToObject<List<object>>();

                    dict[pair.Key] = newList;

                    ConvertJsonTypes(newList);
                }
            }
        }

        private static void ConvertJsonTypes(IList<object> list)
        {
            var newList = list.ToArray();

            for (var i = 0; i < list.Count; i++)
            {
                var jObject = newList[i] as JObject;
                if (jObject != null)
                {
                    var newDict = jObject.ToObject<Dictionary<string, object>>();

                    list[i] = newDict;

                    ConvertJsonTypes(newDict);
                }

                var jArray = newList[i] as JArray;
                if (jArray != null)
                    list[i] = jArray.ToObject<List<object>>();
            }
        }

        /// <summary>
        /// Converts JSON string into map object or returns empty map when conversion is not possible.
        /// </summary>
        /// <param name="value">the JSON string to convert.</param>
        /// <returns>Map object value or empty map when conversion is not supported.</returns>
        public static IDictionary<string, object> ToMap(string value)
        {
            var result = ToNullableMap(value);
            return result ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Converts JSON string into map object or returns default map when conversion is not possible.
        /// </summary>
        /// <param name="value">the JSON string to convert.</param>
        /// <param name="defaultValue">the default value.</param>
        /// <returns>Map object value or default map when conversion is not supported.</returns>
        public static IDictionary<string, object> ToMapWithDefault(string value, IDictionary<string, object> defaultValue)
        {
            var result = ToNullableMap(value);
            return result ?? defaultValue;
        }
    }
}