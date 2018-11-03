using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values into map objects using extended conversion rules.
    /// This class is similar to MapConverter, but is recursively converts all values
    /// stored in objects and arrays.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = RecursiveMapConverted.ToNullableMap("ABC"); // Result: null
    /// var value2 = RecursiveMapConverted.ToNullableMap({ key: 123 }); // Result: { key: 123 }
    /// var result = new List<Object>();
    /// result.Add(1); 
    /// result.Add(new int[]{2, 3});
    /// var value3 = RecursiveMapConverted.ToNullableMap(result); // Result: { "0": 1, { "0": 2, "1": 3 } }
    /// </code>
    /// </example>
public class RecursiveMapConverter
    {
        private static IDictionary<string, object> ObjectToMap(object value)
        {
            if (value == null) return null;

            var result = new Dictionary<string, object>();
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(value))
            {
                var propValue = prop.GetValue(value);

                // Recursive conversion
                propValue = ValueToMap(propValue);

                result.Add(prop.Name, propValue);
            }
            return result;
        }

        private static IDictionary<string, object> ObjectToMap(JObject value)
        {
            if (value == null) return null;

            return value.Properties().ToDictionary(pair => pair.Name, pair => ValueToMap(pair.Value));
        }


        private static object[] ArrayToMap(IEnumerable<object> value)
        {
            var result = value as object[] ?? value.ToArray();

            for (var index = 0; index < result.Length; index++)
                result[index] = ValueToMap(result[index]);

            return result;
        }

        private static IDictionary<string, object> MapToMap(IDictionary<string, object> value)
        {
            var result = new Dictionary<string, object>();

            foreach (var key in value.Keys)
                result[key] = ValueToMap(value[key]);

            return result;
        }

        private static IDictionary<string, object> ObjectMapToMap(IDictionary<object, object> value)
        {
            var result = new Dictionary<string, object>();

            foreach (string key in value.Keys)
                result[key] = ValueToMap(value[key]);

            return result;
        }

        private static object ExtensionToMap(object value)
        {
            if (value == null) return null;

            var valueType = value.GetType().Name;

            // TODO: .NET Core does not support ExtensionDataObject
            // Convert extension objects
#if !CORE_NET
            if (valueType == "ExtensionDataObject")
            {
                var extResult = new Dictionary<string, object>();

                var membersProperty = typeof(ExtensionDataObject).GetProperty(
                    "Members", BindingFlags.NonPublic | BindingFlags.Instance);
                var members = (IList)membersProperty.GetValue(value, null);

                foreach (var member in members)
                {
                    var memberNameProperty = member.GetType().GetProperty("Name");
                    var memberName = (string)memberNameProperty.GetValue(member, null);

                    var memberValueProperty = member.GetType().GetProperty("Value");
                    var memberValue = memberValueProperty.GetValue(member, null);
                    memberValue = ExtensionToMap(memberValue);

                    extResult.Add(memberName, memberValue);
                }

                return extResult;
            }
#endif

            // Convert classes
            if (valueType.StartsWith("ClassDataNode"))
            {
                var classResult = new Dictionary<string, object>();

                var membersProperty = value.GetType().GetTypeInfo().GetProperty(
                    "Members", BindingFlags.NonPublic | BindingFlags.Instance);
                var members = (IList)membersProperty.GetValue(value, null);

                foreach (var member in members)
                {
                    var memberNameProperty = member.GetType().GetTypeInfo().GetProperty("Name");
                    var memberName = (string)memberNameProperty.GetValue(member, null);

                    var memberValueProperty = member.GetType().GetTypeInfo().GetProperty("Value");
                    var memberValue = memberValueProperty.GetValue(member, null);
                    memberValue = ExtensionToMap(memberValue);

                    classResult.Add(memberName, memberValue);
                }

                return classResult;
            }

            // Convert collections and arrays
            if (valueType.StartsWith("CollectionDataNode"))
            {
                var itemsProperty = value.GetType().GetTypeInfo().GetProperty(
                    "Items", BindingFlags.NonPublic | BindingFlags.Instance);
                var items = (IList)itemsProperty.GetValue(value, null);

                var arrayResult = new object[items.Count];

                for (var index = 0; index < items.Count; index++)
                    arrayResult[index] = ExtensionToMap(items[index]);

                return arrayResult;
            }

            // Convert values
            if (valueType.StartsWith("DataNode"))
            {
                var dataValueProperty = value.GetType().GetTypeInfo().GetProperty("Value");
                var valueResult = dataValueProperty.GetValue(value, null);
                valueResult = ExtensionToMap(valueResult);
                return valueResult;
            }

            return value;
        }

        private static object ValueToMap(object value)
        {
            if (value == null) return null;

            // Skip expected non-primitive values
            if (value is string || value is Type) return value;

            var valueType = value.GetType().GetTypeInfo();

            // Skip primitive values
            if (valueType.IsPrimitive || valueType.IsValueType) return value;
            // Skip Json.Net values
            if (valueType.Name == "JValue") return ((JValue) value).Value;

            if (value is IDictionary<string, object>)
                return MapToMap((IDictionary<string, object>)value);

            if (value is IDictionary<object, object>)
                return ObjectMapToMap((IDictionary<object, object>)value);

            // Convert arrays
            if (value is IEnumerable<object> && valueType.Name != "JObject")
                return ArrayToMap((IEnumerable<object>)value);

            // TODO: .NET Core does not support ExtensionDataObject
            // Convert partial updates
#if !CORE_NET
            if (value is IExtensibleDataObject)
                return ExtensionToMap(((IExtensibleDataObject)value).ExtensionData);
#endif

            if (valueType.Name == "JObject")
                return ObjectToMap((JObject) value);

            return ObjectToMap(value);
        }

        /// <summary>
        /// Converts value into map object or returns null when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>map object or null when conversion is not supported.</returns>
        public static IDictionary<string, object> ToNullableMap(object value)
        {
            return ValueToMap(value) as IDictionary<string, object>;
        }

        /// <summary>
        /// Converts value into map object or returns empty map when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>map object or empty map when conversion is not supported.</returns>
        public static IDictionary<string, object> ToMap(object value)
        {
            var result = ToNullableMap(value);
            return result ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Converts value into map object or returns default map when conversion is not possible.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>map object or default map when conversion is not supported.</returns>
        public static IDictionary<string, object> ToMapWithDefault(object value, Dictionary<string, object> defaultValue)
        {
            var result = ToNullableMap(value);
            return result ?? defaultValue;
        }
    }
}