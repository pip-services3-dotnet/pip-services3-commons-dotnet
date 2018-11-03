using System.Collections.Generic;

using PipServices3.Commons.Convert;

namespace PipServices3.Commons.Reflect
{
    /// <summary>
    /// Helper class to perform property introspection and dynamic writing.
    /// 
    /// It is similar to ObjectWriter but writes properties recursively 
    /// through the entire object graph.Nested property names are defined
    /// using dot notation as "object.subobject.property"
    /// </summary>
    /// See <see cref="PropertyReflector"/>, <see cref="ObjectWriter"/>
    public sealed class RecursiveObjectWriter
    {
        private static object CreateProperty(object obj, string[] names, int nameIndex)
        {
            // If next field is index then create an array
            string subField = names.Length > nameIndex + 1 ? names[nameIndex + 1] : null;
            int? subFieldIndex = IntegerConverter.ToNullableInteger(subField);
            if (subFieldIndex != null)
                return new List<object>();

            // Else create a dictionary
            return new Dictionary<string, object>();
        }

        private static void PerformSetProperty(object obj, string[] names, int nameIndex, object value)
        {
            if (nameIndex < names.Length - 1)
            {
                var subObj = ObjectReader.GetProperty(obj, names[nameIndex]);
                if (subObj != null)
                    PerformSetProperty(subObj, names, nameIndex + 1, value);
                else
                {
                    subObj = CreateProperty(obj, names, nameIndex);
                    if (subObj != null)
                    {
                        PerformSetProperty(subObj, names, nameIndex + 1, value);
                        ObjectWriter.SetProperty(obj, names[nameIndex], subObj);
                    }
                }
            }
            else
                ObjectWriter.SetProperty(obj, names[nameIndex], value);
        }

        /// <summary>
        /// Recursively sets value of object and its subobjects property specified by its name.
        /// 
        /// The object can be a user defined object, map or array.The property name
        /// correspondently must be object property, map key or array index.
        /// If the property does not exist or introspection fails this method doesn't do
        /// anything and doesn't any throw errors.
        /// </summary>
        /// <param name="obj">an object to write property to.</param>
        /// <param name="name">a name of the property to set.</param>
        /// <param name="value">a new value for the property to set.</param>
        public static void SetProperty(object obj, string name, object value)
        {
            if (obj == null || name == null) return;

            var names = name.Split('.');
            if (names == null || names.Length == 0)
                return;

            PerformSetProperty(obj, names, 0, value);
        }

        /// <summary>
        /// Recursively sets values of some (all) object and its subobjects properties.
        /// 
        /// The object can be a user defined object, map or array.Property values
        /// correspondently are object properties, map key-pairs or array elements with their indexes.
        /// 
        /// If some properties do not exist or introspection fails they are just silently
        /// skipped and no errors thrown.
        /// </summary>
        /// <param name="obj">an object to write properties to.</param>
        /// <param name="values">a map, containing property names and their values.</param>
        /// See <see cref="SetProperty(object, string, object)"/>
        public static void SetProperties(object obj, IDictionary<string, object> values)
        {
            if (values == null || values.Count == 0) return;

            foreach (var entry in values)
                SetProperty(obj, entry.Key, entry.Value);
        }

        /// <summary>
        /// Copies content of one object to another object by recursively reading all
        /// properties from source object and then recursively writing them to destination object.
        /// </summary>
        /// <param name="dest">a destination object to write properties to.</param>
        /// <param name="src">a source object to read properties from</param>
        public static void CopyProperties(object dest, object src)
        {
            if (dest == null || src == null) return;

            var values = RecursiveObjectReader.GetProperties(src);
            SetProperties(dest, values);
        }
    }
}
