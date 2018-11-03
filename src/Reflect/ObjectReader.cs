using Newtonsoft.Json.Linq;
using PipServices3.Commons.Convert;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace PipServices3.Commons.Reflect
{
    /// <summary>
    /// Helper class to perform property introspection and dynamic reading.
    /// 
    /// In contrast to PropertyReflector which only introspects regular objects,
    /// this ObjectReader is also able to handle maps and arrays.
    /// For maps properties are key-pairs identified by string keys,
    /// For arrays properties are elements identified by integer index.
    /// 
    /// This class has symmetric implementation across all languages supported
    /// by Pip.Services toolkit and used to support dynamic data processing.
    /// 
    /// Because all languages have different casing and case sensitivity rules,
    /// this ObjectReader treats all property names as case insensitive.
    /// </summary>
    /// <example>
    /// <code>
    /// var myObj = new MyObject();
    /// 
    /// var properties = ObjectReader.GetPropertyNames();
    /// ObjectReader.HasProperty(myObj, "myProperty");
    /// var value = PropertyReflector.GetProperty(myObj, "myProperty");
    /// var myMap = new Dictionary<string, object>(){
    ///     {"key1", 123},
    ///     {"key2", "ABC"};
    /// };
    /// 
    /// ObjectReader.HasProperty(myMap, "key1");
    /// var value = ObjectReader.getProperty(myMap, "key1");
    /// 
    /// int[] myArray = new int[] { 1, 2, 3 };
    /// ObjectReader.HasProperty(myArrat, "0");
    /// var value = ObjectReader.GetProperty(myArray, "0");
    /// </code>
    /// </example>
    /// See <see cref="PropertyReflector"/>
    public static class ObjectReader
    {
        /// <summary>
        /// Gets a real object value. If object is a wrapper, it unwraps the value behind it.
        /// Otherwise it returns the same object value.
        /// </summary>
        /// <param name="obj">an object to unwrap.</param>
        /// <returns>an actual (unwrapped) object value.</returns>
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

        /// <summary>
        /// Checks if object has a property with specified name.
        /// The object can be a user defined object, map or array.The property name
        /// correspondently must be object property, map key or array index.
        /// </summary>
        /// <param name="obj">an object to introspect.</param>
        /// <param name="name">a name of the property to check.</param>
        /// <returns>true if the object has the property and false if it doesn't.</returns>
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

        /// <summary>
        /// Gets value of object property specified by its name.
        /// The object can be a user defined object, map or array.The property name
        /// correspondently must be object property, map key or array index.
        /// </summary>
        /// <param name="obj">an object to read property from.</param>
        /// <param name="name">a name of the property to get.</param>
        /// <returns>the property value or null if property doesn't exist or introspection failed.</returns>
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

        /// <summary>
        /// Gets names of all properties implemented in specified object.
        /// 
        /// The object can be a user defined object, map or array.Returned property name
        /// correspondently are object properties, map keys or array indexes.
        /// </summary>
        /// <param name="obj">an objec to introspect.</param>
        /// <returns>a list with property names.</returns>
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

        /// <summary>
        /// Get values of all properties in specified object and returns them as a map. 
        /// The object can be a user defined object, map or array.Returned properties 
        /// correspondently are object properties, map key-pairs or array elements with their indexes.
        /// </summary>
        /// <param name="obj">an object to get properties from.</param>
        /// <returns>a map, containing the names of the object's properties and their values.</returns>
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
