using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PipServices3.Commons.Convert;

namespace PipServices3.Commons.Reflect
{
    /// <summary>
    /// Helper class to perform property introspection and dynamic writing.
    /// 
    /// In contrast to PropertyReflector which only introspects regular objects,
    /// this ObjectWriter is also able to handle maps and arrays.
    /// For maps properties are key-pairs identified by string keys,
    /// For arrays properties are elements identified by integer index.
    /// 
    /// This class has symmetric implementation across all languages supported
    /// by Pip.Services toolkit and used to support dynamic data processing.
    /// 
    /// Because all languages have different casing and case sensitivity rules,
    /// this ObjectWriter treats all property names as case insensitive.
    /// </summary>
    /// <example>
    /// <code>
    /// var myObj = new MyObject();
    /// 
    /// ObjectWriter.SetProperty(myObj, "myProperty", 123);
    /// 
    /// var myMap = new Dictionary<string, object>(){
    ///     {"key1", 123},
    ///     {"key2", "ABC"}
    /// };
    /// 
    /// ObjectWriter.SetProperty(myMap, "key1", "XYZ");
    /// 
    /// var myArray = new int[] { 1, 2, 3 };
    /// ObjectWriter.SetProperty(myArray, "0", 123);
    /// </code>
    /// </example>
    /// See <see cref="PropertyReflector"/>
    public class ObjectWriter
    {
        /// <summary>
        /// Sets value of object property specified by its name.
        /// 
        /// The object can be a user defined object, map or array.The property name
        /// correspondently must be object property, map key or array index.
        /// 
        /// If the property does not exist or introspection fails this method doesn't do
        /// anything and doesn't any throw errors.
        /// </summary>
        /// <param name="obj">an object to write property to.</param>
        /// <param name="name">a name of the property to set.</param>
        /// <param name="value">a new value for the property to set.</param>
        public static void SetProperty(object obj, string name, object value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Object cannot be null");
            if (name == null)
                throw new ArgumentNullException(nameof(name), "Method name cannot be null");

            var type = obj.GetType().GetTypeInfo();
            var isDict = type.GetInterfaces().Contains(typeof(IDictionary)) || type.GetInterfaces().Contains(typeof(IDictionary<,>));

            if (isDict)
            {
                var mapObj = (IDictionary<string, object>)obj;

                foreach (var key in mapObj.Keys)
                {
                    if (name.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        mapObj[key] = value;
                        return;
                    }
                }

                mapObj[name] = value;
            }
            else if (obj.GetType().IsArray)
            {
                var array = ((Array)obj);
                var length = array.Length;
                var index = IntegerConverter.ToIntegerWithDefault(name, -1);

                if (index >= 0 && index < length)
                {
                    array.SetValue(value, index);
                }
            }
            else if (type.GetInterfaces().Contains(typeof(IList)) || type.GetInterfaces().Contains(typeof(IList<>)))
            {
                var list = (IList<object>)obj;
                var index = IntegerConverter.ToIntegerWithDefault(name, -1);
                if (index < 0)
                    return;

                if (index < list.Count)
                    list[index] = value;
                else
                {
                    while (index - 1 >= list.Count)
                        list.Add(null);
                    list.Add(value);
                }
            }
            else
            {
                PropertyReflector.SetProperty(obj, name, value);
            }
        }

        /// <summary>
        /// Sets values of some (all) object properties.
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
            if (values == null || values.Count == 0)
                return;

            foreach (var entry in values)
            {
                SetProperty(obj, entry.Key, entry.Value);
            }
        }
    }
}
