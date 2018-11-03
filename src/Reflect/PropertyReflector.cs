using System;
using System.Collections.Generic;
using System.Reflection;

namespace PipServices3.Commons.Reflect
{
    /// <summary>
    /// Helper class to perform property introspection and dynamic reading and writing.
    /// 
    /// This class has symmetric implementation across all languages supported 
    /// by Pip.Services toolkit and used to support dynamic data processing.
    /// 
    /// Because all languages have different casing and case sensitivity rules,
    /// this PropertyReflector treats all property names as case insensitive.
    /// </summary>
    /// <example>
    /// <code>
    /// var myObj = new MyObject();
    /// 
    /// var properties = PropertyReflector.GetPropertyNames();
    /// PropertyReflector.HasProperty(myObj, "myProperty");
    /// var value = PropertyReflector.GetProperty(myObj, "myProperty");
    /// PropertyReflector.SetProperty(myObj, "myProperty", 123);
    /// </code>
    /// </example>
    public class PropertyReflector
    {
        private static bool MatchField(FieldInfo field, string name)
        {
            return field.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                && field.IsPublic && !field.IsStatic;
        }

        private static bool MatchPropertyGetter(PropertyInfo property, string name)
        {
            var method = property.GetGetMethod();

            return property.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                && method.IsPublic && !method.IsStatic
                && !method.IsAbstract;
        }

        private static bool MatchPropertySetter(PropertyInfo property, string name)
        {
            var method = property.GetSetMethod();
            return property.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                && method.IsPublic && !method.IsStatic
                && !method.IsAbstract;
        }

        /// <summary>
        /// Checks if object has a property with specified name.
        /// </summary>
        /// <param name="obj">an object to introspect.</param>
        /// <param name="name">a name of the property to check.</param>
        /// <returns>true if the object has the property and false if it doesn't.</returns>
        public static bool HasProperty(object obj, string name)
        {
            if (obj == null)
                throw new NullReferenceException("Object cannot be null");
            if (name == null)
                throw new NullReferenceException("Property name cannot be null");

            var objType = obj.GetType();

            // Search in fields
            foreach (var field in objType.GetRuntimeFields())
            {
                if (MatchField(field, name))
                    return true;
            }

            // Search in properties
            foreach (var property in objType.GetRuntimeProperties())
            {
                if (MatchPropertyGetter(property, name))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets value of object property specified by its name.
        /// </summary>
        /// <param name="obj">an object to read property from.</param>
        /// <param name="name">a name of the property to get.</param>
        /// <returns>the property value or null if property doesn't exist or introspection failed.</returns>
        public static object GetProperty(object obj, string name)
        {
            if (obj == null)
                throw new NullReferenceException("Object cannot be null");
            if (name == null)
                throw new NullReferenceException("Property name cannot be null");

            var objType = obj.GetType();

            // Search in fields
            foreach (var field in objType.GetRuntimeFields())
            {
                try
                {
                    if (MatchField(field, name))
                        return field.GetValue(obj);
                }
                catch (Exception)
                {
                    // Ignore exception
                }
            }

            // Search in properties
            foreach (var property in objType.GetRuntimeProperties())
            {
                try
                {
                    if (MatchPropertyGetter(property, name))
                        return property.GetValue(obj);
                }
                catch (Exception)
                {
                    // Ignore exception
                }
            }

            return null;
        }

        /// <summary>
        /// Gets names of all properties implemented in specified object.
        /// </summary>
        /// <param name="obj">an objec to introspect.</param>
        /// <returns>a list with property names.</returns>
        public static List<string> GetPropertyNames(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("Object cannot be null");

            var properties = new List<string>();

            var objType = obj.GetType();

            // Get all fields
            foreach (var field in objType.GetRuntimeFields())
            {
                if (MatchField(field, field.Name))
                    properties.Add(field.Name);
            }

            // Get all properties
            foreach (var property in objType.GetRuntimeProperties())
            {
                if (MatchPropertyGetter(property, property.Name))
                    properties.Add(property.Name);
            }

            return properties;
        }

        /// <summary>
        /// Get values of all properties in specified object and returns them as a map.
        /// </summary>
        /// <param name="obj">an object to get properties from.</param>
        /// <returns>a map, containing the names of the object's properties and their values.</returns>
        public static Dictionary<string, object> GetProperties(object obj)
        {
            var map = new Dictionary<string, object>();

            var objType = obj.GetType();

            // Get all fields
            foreach (var field in objType.GetRuntimeFields())
            {
                try
                {
                    if (MatchField(field, field.Name))
                        map.Add(field.Name, field.GetValue(obj));
                }
                catch (Exception)
                {
                    // Ignore exception
                }
            }

            // Get all properties
            foreach (var property in objType.GetRuntimeProperties())
            {
                try
                {
                    if (MatchPropertyGetter(property, property.Name))
                        map.Add(property.Name, property.GetValue(obj));
                }
                catch (Exception)
                {
                    // Ignore exception
                }
            }

            return map;
        }

        /// <summary>
        /// Sets value of object property specified by its name.
        /// If the property does not exist or introspection fails this method doesn't do
        /// anything and doesn't any throw errors.
        /// </summary>
        /// <param name="obj">an object to write property to.</param>
        /// <param name="name">a name of the property to set.</param>
        /// <param name="value">a new value for the property to set.</param>
        public static void SetProperty(object obj, string name, object value)
        {
            if (obj == null)
                throw new NullReferenceException("Object cannot be null");
            if (name == null)
                throw new NullReferenceException("Property name cannot be null");

            var objType = obj.GetType();

            // Search in fields
            foreach (var field in objType.GetRuntimeFields())
            {
                try
                {
                    if (MatchField(field, name))
                    {
                        field.SetValue(obj, value);
                        return;
                    }
                }
                catch (Exception)
                {
                    // Ignore exception
                }
            }

            // Search in properties
            foreach (var property in objType.GetRuntimeProperties())
            {
                try
                {
                    if (MatchPropertyGetter(property, name))
                    {
                        property.SetValue(obj, value);
                    }
                }
                catch (Exception)
                {
                    // Ignore exception
                }
            }
        }

        /// <summary>
        /// Sets values of some (all) object properties.
        /// If some properties do not exist or introspection fails they are just silently
        /// skipped and no errors thrown.
        /// </summary>
        /// <param name="obj">an object to write properties to.</param>
        /// <param name="values">a map, containing property names and their values.</param>
        /// See <see cref="SetProperty(object, string, object)"/>
        public static void SetProperties(object obj, Dictionary<string, object> values)
        {
            if (values == null || values.Count == 0) return;

            foreach (var entry in values)
            {
                SetProperty(obj, entry.Key, entry.Value);
            }
        }
    }
}