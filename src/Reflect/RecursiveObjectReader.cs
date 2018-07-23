using System.Collections.Generic;
using PipServices.Commons.Convert;
using TypeCode = PipServices.Commons.Convert.TypeCode;

namespace PipServices.Commons.Reflect
{
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
