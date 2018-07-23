using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Converts objects to/from Json format.
    /// </summary>
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

        public static string ToJson(object value)
        {
            if (value == null) return null;
            return JsonConvert.SerializeObject(
                value,
                Formatting.None, 
                JsonSettings
            );
        }

        public static object FromJson(string value)
        {
            if (value == null) return null;
            return JsonConvert.DeserializeObject(value);
        }

        public static T FromJson<T>(string value)
        {
            if (value == null) return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }

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

        public static IDictionary<string, object> ToMap(string value)
        {
            var result = ToNullableMap(value);
            return result ?? new Dictionary<string, object>();
        }

        public static IDictionary<string, object> ToMapWithDefault(string value, IDictionary<string, object> defaultValue)
        {
            var result = ToNullableMap(value);
            return result ?? defaultValue;
        }
    }
}