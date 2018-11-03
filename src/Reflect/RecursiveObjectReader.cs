using System.Collections.Generic;
using PipServices3.Commons.Convert;
using TypeCode = PipServices3.Commons.Convert.TypeCode;

namespace PipServices3.Commons.Reflect
{
    /// <summary>
    /// Helper class to perform property introspection and dynamic reading.
    /// 
    /// It is similar to ObjectReader but reads properties recursively
    /// through the entire object graph.Nested property names are defined
    /// using dot notation as "object.subobject.property"
    /// </summary>
    /// See <see cref="PropertyReflector"/>, <see cref="ObjectReader"/>
    public sealed class RecursiveObjectReader
    {
        private static bool PerformHasProperty(object obj, string[] names, int nameIndex)
        {
            if (nameIndex < names.Length - 1)
            {
                var value = ObjectReader.GetProperty(obj, names[nameIndex]);
                if (value != null)
                    return PerformHasProperty(value, names, nameIndex + 1);

                return false;
            }

            return ObjectReader.HasProperty(obj, names[nameIndex]);
        }

        /// <summary>
        /// Checks recursively if object or its subobjects has a property with specified name.
        /// 
        /// The object can be a user defined object, map or array.The property name
        /// correspondently must be object property, map key or array index.
        /// </summary>
        /// <param name="obj">an object to introspect.</param>
        /// <param name="name">a name of the property to check.</param>
        /// <returns>true if the object has the property and false if it doesn't.</returns>
        public static bool HasProperty(object obj, string name)
        {
            if (obj == null || name == null) return false;

            var names = name.Split('.');
            if (names == null || names.Length == 0)
                return false;

            return PerformHasProperty(obj, names, 0);
        }

        private static object PerformGetProperty(object obj, string[] names, int nameIndex)
        {
            if (nameIndex < names.Length - 1)
            {
                var value = ObjectReader.GetProperty(obj, names[nameIndex]);
                if (value != null)
                    return PerformGetProperty(value, names, nameIndex + 1);

                return null;
            }

            return ObjectReader.GetProperty(obj, names[nameIndex]);
        }

        /// <summary>
        /// Recursively gets value of object or its subobjects property specified by its name.
        /// The object can be a user defined object, map or array.The property name
        /// correspondently must be object property, map key or array index.
        /// </summary>
        /// <param name="obj">an object to read property from.</param>
        /// <param name="name">a name of the property to get.</param>
        /// <returns>the property value or null if property doesn't exist or introspection failed.</returns>
        public static object GetProperty(object obj, string name)
        {
            if (obj == null || name == null) return null;

            var names = name.Split('.');
            if (names == null || names.Length == 0)
                return null;

            return PerformGetProperty(obj, names, 0);
        }

        private static bool IsSimpleValue(object value)
        {
            var code = TypeConverter.ToTypeCode(value);
            return code != TypeCode.Array && code != TypeCode.Map && code != TypeCode.Object;
        }

        private static void PerformGetPropertyNames(object obj, string path,
            IList<string> result, IList<object> cycleDetect)
        {
            var map = ObjectReader.GetProperties(obj);

            if (map.Count != 0 && cycleDetect.Count < 100)
            {
                cycleDetect.Add(obj);
                try
                {
                    foreach (var entry in map)
                    {
                        var value = entry.Value;

                        // Prevent cycles 
                        if (cycleDetect.Contains(value))
                            continue;

                        var key = path != null ? path + "." + entry.Key : entry.Key;

                        // Add simple values directly
                        if (IsSimpleValue(value))
                            result.Add(key);
                        // Recursively go to elements
                        else
                            PerformGetPropertyNames(value, key, result, cycleDetect);
                    }
                }
                finally
                {
                    cycleDetect.Remove(obj);
                }
            }
            else
            {
                if (path != null)
                    result.Add(path);
            }
        }

        /// <summary>
        /// Recursively gets names of all properties implemented in specified object and its subobjects.
        /// 
        /// The object can be a user defined object, map or array.Returned property name
        /// correspondently are object properties, map keys or array indexes.
        /// </summary>
        /// <param name="obj">an objec to introspect.</param>
        /// <returns>a list with property names.</returns>
        public static IList<string> GetPropertyNames(object obj)
        {
            var propertyNames = new List<string>();

            if (obj == null)
            {
                return propertyNames;
            }
            else
            {
                var cycleDetect = new List<object>();
                PerformGetPropertyNames(obj, null, propertyNames, cycleDetect);
                return propertyNames;
            }
        }

        private static void PerformGetProperties(object obj, string path,
            IDictionary<string, object> result, ICollection<object> cycleDetect)
        {
            var map = ObjectReader.GetProperties(obj);

            if (map.Count != 0 && cycleDetect.Count < 100)
            {
                cycleDetect.Add(obj);
                try
                {
                    foreach(var entry in map)
                    {
                        var value = entry.Value;

                        // Prevent cycles 
                        if (cycleDetect.Contains(value))
                            continue;

                        var key = path != null ? path + "." + entry.Key : entry.Key;

                        // Add simple values directly
                        if (IsSimpleValue(value))
                            result[key] = value;
                        // Recursively go to elements
                        else
                            PerformGetProperties(value, key, result, cycleDetect);
                    }
                }
                finally
                {
                    cycleDetect.Remove(obj);
                }
            }
            else
            {
                if (path != null)
                    result[path] = obj;
            }
        }

        /// <summary>
        /// Get values of all properties in specified object and its subobjects and returns them as a map.
        /// 
        /// The object can be a user defined object, map or array.Returned properties
        /// correspondently are object properties, map key-pairs or array elements with their indexes.
        /// </summary>
        /// <param name="obj">an object to get properties from.</param>
        /// <returns>a map, containing the names of the object's properties and their values.</returns>
        public static IDictionary<string, object> GetProperties(object obj)
        {
            var properties = new Dictionary<string, object>();

            if (obj == null)
                return properties;

            var cycleDetect = new List<object>();
            PerformGetProperties(obj, null, properties, cycleDetect);
            return properties;

        }
    }
}
