using System;
using System.Collections.Generic;
using System.Reflection;

namespace PipServices.Commons.Reflect
{
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