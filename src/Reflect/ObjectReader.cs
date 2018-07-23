using Newtonsoft.Json.Linq;
using PipServices.Commons.Convert;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace PipServices.Commons.Reflect
{
    public static class ObjectReader
    {
        public static object GetValue(object obj)
        {
            if (obj is JValue)
            {
                var value = (JValue)obj;
                return value.Value;
            }

            if (obj is JObject)
            {
                var thisObj = (JObject)obj;
                var map = new Dictionary<string, object>();
                foreach (var property in thisObj.Properties())
                    map[property.Name] = property.Value;
                return map;
            }

            if (obj is JArray)
            {
                var thisObj = (JArray)obj;
                var list = new List<object>();
                foreach (var element in thisObj)
                    list.Add(element);
                return list;
            }

            return obj;
        }

        public static bool HasProperty(object obj, string name)
        {
            if (obj == null || name == null)
                return false;

            var jObject = obj as JObject;
            if (jObject != null)
            {
                var thisObj = jObject;
                foreach (var property in thisObj.Properties())
                {
                    if (name.Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                return false;
            }

            var jArray = obj as JArray;
            if (jArray != null)
            {
                var list = jArray;
                var index = IntegerConverter.ToNullableInteger(name);
                return index >= 0 && index < list.Count;
            }

            var map = obj as IDictionary;
            if (map != null)
            {
                foreach (var key in map.Keys)
                {
                    if (name.Equals(key.ToString(), StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                return false;
            }

            var genDictionary = obj as IDictionary<string, object>;
            if (genDictionary != null)
            {
                foreach (var key in genDictionary.Keys)
                {
                    if (name.Equals(key, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                return false;
            }

            var enumerable = obj as IEnumerable;
            if (enumerable != null)
            {
                var index = IntegerConverter.ToNullableInteger(name);
                var genericEnum = enumerable.Cast<object>();
                return index >= 0 && index < genericEnum.Count();
            }

            return PropertyReflector.HasProperty(obj, name);
        }

        public static object GetProperty(object obj, string name)
        {
            if (obj == null || name == null)
                return false;

            name = name.ToLower();

            var jObject = obj as JObject;
            if (jObject != null)
            {
                var thisObj = jObject;
                foreach (var property in thisObj.Properties())
                {
                    if (name.Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                        return property.Value;
                }
                return null;
            }

            var jArray = obj as JArray;
            if (jArray != null)
            {
                var list = jArray;
                var index = IntegerConverter.ToNullableInteger(name);
                return index >= 0 && index < list.Count ? list[index.Value] : null;
            }

            var map = obj as IDictionary;
            if (map != null)
            {
                foreach (var key in map.Keys)
                {
                    if (name.Equals(key.ToString(), StringComparison.OrdinalIgnoreCase))
                        return map[key];
                }
            }

            var genDictionary = obj as IDictionary<string, object>;
            if (genDictionary != null)
            {
                foreach (var key in genDictionary.Keys)
                {
                    if (name.Equals(key, StringComparison.OrdinalIgnoreCase))
                        return genDictionary[key];
                }
            }

            var enumerable = obj as IEnumerable;
            if (enumerable != null)
            {
                var index = IntegerConverter.ToNullableInteger(name);
                if (index >= 0)
                {
                    var list = (IEnumerable)obj;
                    foreach (var value in list)
                    {
                        if (index == 0)
                            return value;
                        index--;
                    }
                }
                return null;
            }

            return PropertyReflector.GetProperty(obj, name);
        }

        public static List<string> GetPropertyNames(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("Object cannot be null");

            var properties = new List<string>();

            if (obj is JObject)
            {
                var thisObj = (JObject)obj;
                foreach (var property in thisObj.Properties())
                {
                    properties.Add(property.Name);
                }
            }
            else if (obj is JArray)
            {
                var list = (JArray)obj;
                for (var index = 0; index < list.Count; index++)
                {
                    properties.Add(index.ToString());
                }
            }
            else if (obj is IDictionary)
            {
                var map = (IDictionary)obj;
                foreach (var key in map.Keys)
                {
                    properties.Add(key.ToString());
                }
            }
            else if (obj is IDictionary<string, object>)
            {
                var map = (IDictionary<string, object>)obj;
                foreach (var key in map.Keys)
                {
                    properties.Add(key);
                }
            }
            else if (obj is IEnumerable)
            {
                var list = (IEnumerable)obj;
                var index = 0;
                foreach (var value in list)
                {
                    properties.Add(index.ToString());
                    index++;
                }
            }
            else
            {
                return PropertyReflector.GetPropertyNames(obj);
            }

            return properties;
        }

        public static Dictionary<string, object> GetProperties(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("Object cannot be null");

            var map = new Dictionary<string, object>();

            if (obj is JObject)
            {
                var thisObj = (JObject)obj;
                foreach (var property in thisObj.Properties())
                {
                    map[property.Name] = property.Value;
                }
            }
            else if (obj is JArray)
            {
                var list = (JArray)obj;
                for (var index = 0; index < list.Count; index++)
                {
                    map[index.ToString()] = list[index];
                }
            }
            else if (obj is IDictionary)
            {
                var thisMap = (IDictionary)obj;
                foreach (var key in thisMap.Keys)
                {
                    map[key.ToString()] = thisMap[key];
                }
            }
            else if (obj is IDictionary<string, object>)
            {
                var thisMap = (IDictionary<string, object>)obj;
                foreach (var key in thisMap.Keys)
                {
                    map[key] = thisMap[key];
                }
            }
            else if (obj is IEnumerable)
            {
                var list = (IEnumerable)obj;
                var index = 0;
                foreach (var value in list)
                {
                    map[index.ToString()] = value;
                    index++;
                }
            }
            else
            {
                return PropertyReflector.GetProperties(obj);
            }

            return map;
        }
    }
}
