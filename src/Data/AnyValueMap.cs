using PipServices3.Commons.Convert;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Cross-language implementation of dynamic object map (dictionary) what can hold values of any type.
    /// The stored values can be converted to different types using variety of accessor methods.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = new AnyValueMap(new Dictionary<string, object>{
    ///     {"key1", 1},
    ///     {"key2", "123.456"},
    ///     {"key3", "2018-01-01" }
    ///     });
    /// 
    /// value1.GetAsBoolean("key1");   // Result: true
    /// value1.GetAsInteger("key2");   // Result: 123
    /// value1.GetAsFloat("key2");     // Result: 123.456
    /// value1.GetAsDateTime("key3");  // Result: new Date(2018,0,1)
    /// </code>
    /// </example>
    /// See <see cref="StringConverter"/>, <see cref="BooleanConverter"/>, <see cref="IntegerConverter"/>, 
    /// <see cref="LongConverter"/>, <see cref="DoubleConverter"/>, <see cref="FloatConverter"/>, 
    /// <see cref="DateTimeConverter"/>, <see cref="ICloneable"/>
    public class AnyValueMap : Dictionary<string, object>, ICloneable
    {
        /// <summary>
        /// Creates a new instance of the map and assigns its value.
        /// </summary>
        public AnyValueMap()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Creates a new instance of the map and assigns its value.
        /// </summary>
        /// <param name="values">(optional) values to initialize this map.</param>
        public AnyValueMap(IDictionary<string, object> values)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            Append(values);
        }

        /// <summary>
        /// Creates a new instance of the map and assigns its value.
        /// </summary>
        /// <param name="values">(optional) values to initialize this map.</param>
        public AnyValueMap(IDictionary values)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            Append(values);
        }

        public new object this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        /// <summary>
        /// Gets a map element specified by its key.
        /// </summary>
        /// <param name="key">a key of the element to get.</param>
        /// <returns>the value of the map element.</returns>
        public virtual object Get(string key)
        {
            object value = null;
            base.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Sets a new value for this map element
        /// </summary>
        /// <param name="key">(optional) a key of the element to set</param>
        /// <param name="value">a new element or map value.</param>
        public virtual void Set(string key, object value)
        {
            base[key] = value;
        }

        /// <summary>
        /// Appends new elements to this map.
        /// </summary>
        /// <param name="map">a map with elements to be added.</param>
        public void Append(IDictionary<string, object> map)
        {
            if (map == null || map.Count == 0) return;

            foreach (var key in map.Keys)
                Set(key, map[key]);
        }

        /// <summary>
        /// Appends new elements to this map.
        /// </summary>
        /// <param name="map">a map with elements to be added.</param>
        public void Append(IDictionary map)
        {
            if (map == null || map.Count == 0) return;

            foreach (var key in map.Keys)
                Set(key.ToString(), map[key]);
        }

        /// <summary>
        ///  Gets the value stored in this map element without any conversions
        /// </summary>
        /// <returns>the value of the map element.</returns>
        public object GetAsObject()
        {
            return new Dictionary<string, object>(this);
        }

        /// <summary>
        /// Gets the value stored in map element without any conversions. When element key is not defined it returns the entire map value.
        /// </summary>
        /// <param name="key">(optional) a key of the element to get</param>
        /// <returns>the element value or value of the map when index is not defined.</returns>
        public object GetAsObject(string key)
        {
            return Get(key);
        }

        /// <summary>
        /// Sets a new value for this array element
        /// </summary>
        /// <param name="value">the new object value.</param>
        public void SetAsObject(object value)
        {
            Clear();
            Append((IDictionary)MapConverter.ToMap(value));
        }

        /// <summary>
        /// Sets a new value to map element specified by its index. When the index is not
        /// defined, it resets the entire map value.This method has double purpose
        /// because method overrides are not supported in JavaScript.
        /// </summary>
        /// <param name="key">(optional) a key of the element to set</param>
        /// <param name="value">a new element or map value.</param>
        public void SetAsObject(string key, object value)
        {
            Set(key, value);
        }

        /// <summary>
        /// Converts map element into a string or returns null if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>string value of the element or null if conversion is not supported.</returns>
        /// See <see cref="StringConverter.ToNullableString(object)"/>
        public string GetAsNullableString(string key)
        {
            var value = Get(key);
            return StringConverter.ToNullableString(value);
        }

        /// <summary>
        /// Converts map element into a string or returns "" if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>string value of the element or "" if conversion is not supported.</returns>
        /// See <see cref="GetAsStringWithDefault(string, string)"/>
        public string GetAsString(string key)
        {
            return GetAsStringWithDefault(key, null);
        }

        /// <summary>
        /// Converts map element into a string or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>string value of the element or default value if conversion is not supported.</returns>
        /// See <see cref="StringConverter.ToStringWithDefault(object, string)"/>
        public string GetAsStringWithDefault(string key, string defaultValue)
        {
            var value = Get(key);
            return StringConverter.ToStringWithDefault(value, defaultValue);
        }

        /// <summary>
        /// Converts map element into a boolean or returns null if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>boolean value of the element or null if conversion is not supported.</returns>
        /// See <see cref="BooleanConverter.ToNullableBoolean(object)"/>
        public bool? GetAsNullableBoolean(string key)
        {
            var value = Get(key);
            return BooleanConverter.ToNullableBoolean(value);
        }

        /// <summary>
        /// Converts map element into a boolean or returns false if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>boolean value of the element or false if conversion is not supported.</returns>
        /// See <see cref="GetAsBooleanWithDefault(string, bool)"/>
        public bool GetAsBoolean(string key)
        {
            return GetAsBooleanWithDefault(key, false);
        }

        /// <summary>
        /// Converts map element into a boolean or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>boolean value of the element or default value if conversion is not supported.</returns>
        /// See <see cref="BooleanConverter.ToBooleanWithDefault(object, bool)"/>
        public bool GetAsBooleanWithDefault(string key, bool defaultValue)
        {
            var value = Get(key);
            return BooleanConverter.ToBooleanWithDefault(value, defaultValue);
        }

        /// <summary>
        /// Converts map element into an integer or returns null if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>integer value of the element or null if conversion is not supported.</returns>
        /// See <see cref="IntegerConverter.ToNullableInteger(object)"/>
        public int? GetAsNullableInteger(string key)
        {
            var value = Get(key);
            return IntegerConverter.ToNullableInteger(value);
        }

        /// <summary>
        /// Converts map element into an integer or returns 0 if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>integer value of the element or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsIntegerWithDefault(string, int)"/>
        public int GetAsInteger(string key)
        {
            return GetAsIntegerWithDefault(key, 0);
        }

        /// <summary>
        /// Converts map element into an integer or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>integer value of the element or default value if conversion is not supported.</returns>
        /// See <see cref="IntegerConverter.ToIntegerWithDefault(object, int)"/>
        public int GetAsIntegerWithDefault(string key, int defaultValue)
        {
            var value = Get(key);
            return IntegerConverter.ToIntegerWithDefault(value, defaultValue);
        }

        /// <summary>
        /// Converts map element into a long or returns null if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>long value of the element or null if conversion is not supported.</returns>
        /// See <see cref="LongConverter.ToNullableLong(object)"/>
        public long? GetAsNullableLong(string key)
        {
            var value = Get(key);
            return LongConverter.ToNullableLong(value);
        }

        /// <summary>
        /// Converts map element into a long or returns 0 if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>long value of the element or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsLongWithDefault(string, long)"/>
        public long GetAsLong(string key)
        {
            return GetAsLongWithDefault(key, 0);
        }

        /// <summary>
        /// Converts map element into a long or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>long value of the element or default value if conversion is not supported.</returns>
        /// See <see cref="LongConverter.ToLongWithDefault(object, long)"/>
        public long GetAsLongWithDefault(string key, long defaultValue)
        {
            var value = Get(key);
            return LongConverter.ToLongWithDefault(value, defaultValue);
        }

        /// <summary>
        /// Converts map element into a float or returns null if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>float value of the element or null if conversion is not supported.</returns>
        /// See <see cref="FloatConverter.ToNullableFloat(object)"/>
        public float? GetAsNullableFloat(string key)
        {
            var value = Get(key);
            return FloatConverter.ToNullableFloat(value);
        }

        /// <summary>
        /// Converts map element into a float or returns 0 if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>float value of the element or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsFloatWithDefault(string, float)"/>
        public float GetAsFloat(string key)
        {
            return GetAsFloatWithDefault(key, 0);
        }

        /// <summary>
        /// Converts map element into a float or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>float value of the element or default value if conversion is not supported.</returns>
        /// See <see cref="FloatConverter.ToFloatWithDefault(object, float)"/>
        public float GetAsFloatWithDefault(string key, float defaultValue)
        {
            var value = Get(key);
            return FloatConverter.ToFloatWithDefault(value, defaultValue);
        }

        /// <summary>
        /// Converts map element into a double or returns null if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>double value of the element or null if conversion is not supported.</returns>
        /// See <see cref="DoubleConverter.ToNullableDouble(object)"/>
        public double? GetAsNullableDouble(string key)
        {
            var value = Get(key);
            return DoubleConverter.ToNullableDouble(value);
        }

        /// <summary>
        /// Converts map element into a double or returns 0 if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>double value of the element or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsDoubleWithDefault(string, double)"/>
        public double GetAsDouble(string key)
        {
            return GetAsDoubleWithDefault(key, 0);
        }

        /// <summary>
        /// Converts map element into a double or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>double value of the element or default value if conversion is not supported.</returns>
        /// See <see cref="DoubleConverter.ToDoubleWithDefault(object, double)"/>
        public double GetAsDoubleWithDefault(string key, double defaultValue)
        {
            var value = Get(key);
            return DoubleConverter.ToDoubleWithDefault(value, defaultValue);
        }

        /// <summary>
        /// Converts map element into a Date or returns null if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>DateTime value of the element or null if conversion is not supported.</returns>
        /// See <see cref="DateTimeConverter.ToNullableDateTime(object)"/>
        public DateTime? GetAsNullableDateTime(string key)
        {
            var value = Get(key);
            return DateTimeConverter.ToNullableDateTime(value);
        }

        /// <summary>
        /// Converts map element into a Date or returns current date if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>DateTime value of the element or current date if conversion is not supported.</returns>
        /// See <see cref="GetAsDateTimeWithDefault(string, DateTime?)"/>
        public DateTime GetAsDateTime(string key)
        {
            return GetAsDateTimeWithDefault(key, new DateTime());
        }

        /// <summary>
        /// Converts map element into a Date or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>DateTime value of the element or default value if conversion is not supported.</returns>
        /// See <see cref="DateTimeConverter.ToDateTimeWithDefault(object, DateTime?)"/>
        public DateTime GetAsDateTimeWithDefault(string key, DateTime? defaultValue)
        {
            var value = Get(key);
            return DateTimeConverter.ToDateTimeWithDefault(value, defaultValue);
        }

        public TimeSpan? GetAsNullableTimeSpan(string key)
        {
            var value = Get(key);
            return TimeSpanConverter.ToNullableTimeSpan(value);
        }

        public TimeSpan GetAsTimeSpan(string key)
        {
            return GetAsTimeSpanWithDefault(key, new TimeSpan(0));
        }

        public TimeSpan GetAsTimeSpanWithDefault(string key, TimeSpan? defaultValue)
        {
            var value = Get(key);
            return TimeSpanConverter.ToTimeSpanWithDefault(value, defaultValue);
        }

        public T? GetAsNullableEnum<T>(string key) where T : struct
        {
            var value = Get(key);
            return EnumConverter.ToNullableEnum<T>(value);
        }

        public T GetAsEnum<T>(string key)
        {
            return GetAsEnumWithDefault<T>(key, default(T));
        }

        public T GetAsEnumWithDefault<T>(string key, T defaultValue)
        {
            var value = Get(key);
            return EnumConverter.ToEnumWithDefault<T>(value, defaultValue);
        }

        /// <summary>
        /// Converts map element into a value defined by specied typecode. If conversion 
        /// is not possible it returns default value for the specied typecode.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="key">a key of element to get.</param>
        /// <returns>element value defined by the typecode or default value if conversion is not supported.</returns>
        /// See <see cref="TypeConverter.ToType{T}(object)"/>
        public T GetAsType<T>(string key)
        {
            var value = Get(key);
            return TypeConverter.ToType<T>(value);
        }

        /// <summary>
        /// Converts map element into a value defined by specied typecode. If conversion 
        /// is not possible it returns null.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="key">a key of element to get.</param>
        /// <returns>element value defined by the typecode or null if conversion is not supported.</returns>
        /// See <see cref="TypeConverter.ToNullableType{T}(object)"/>
        public T? GetAsNullableType<T>(string key) where T : struct
        {
            var value = Get(key);
            return TypeConverter.ToNullableType<T>(value);
        }

        /// <summary>
        /// Converts map element into a value defined by specied typecode. If conversion 
        /// is not possible it returns default value.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="key">a key of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>element value defined by the typecode or default value if conversion is not supported.</returns>
        /// See <see cref="TypeConverter.ToTypeWithDefault{T}(object, T)"/>
        public T GetAsTypeWithDefault<T>(string key, T defaultValue)
        {
            var value = Get(key);
            return TypeConverter.ToTypeWithDefault<T>(value, defaultValue);
        }

        /// <summary>
        /// Converts map element into an AnyValue or returns an empty AnyValue if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>AnyValue value of the element or empty AnyValue if conversion is not supported.</returns>
        /// See <see cref="AnyValue"/>
        public AnyValue GetAsValue(string key)
        {
            return new AnyValue(GetAsObject(key));
        }

        /// <summary>
        /// Converts map element into an AnyValueArray or returns null if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>AnyValueArray value of the element or null if conversion is not supported.</returns>
        /// See <see cref="AnyValueArray"/>
        public AnyValueArray GetAsNullableArray(string key)
        {
            var value = GetAsObject(key);
            return value != null ? AnyValueArray.FromValue(value) : null;
        }

        /// <summary>
        /// Converts map element into an AnyValueArray or returns empty AnyValueArray if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>AnyValueArray value of the element or empty AnyValueArray if conversion is not supported.</returns>
        /// See <see cref="AnyValueArray.FromValue(object)"/>
        public AnyValueArray GetAsArray(string key)
        {
            var value = GetAsObject(key);
            return AnyValueArray.FromValue(key);
        }

        /// <summary>
        /// Converts map element into an AnyValueArray or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>AnyValueArray value of the element or default value if conversion is not supported.</returns>
        /// See <see cref="GetAsNullableArray(string)"/>
        public AnyValueArray GetAsArrayWithDefault(string key, AnyValueArray defaultValue)
        {
            var result = GetAsNullableArray(key);
            return result ?? defaultValue;
        }

        /// <summary>
        /// Converts map element into an AnyValueMap or returns null if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>AnyValueMap value of the element or null if conversion is not supported.</returns>
        /// See <see cref="FromValue(object)"/>
        public AnyValueMap GetAsNullableMap(string key)
        {
            var value = GetAsObject(key);
            return value != null ? FromValue(value) : null;
        }

        /// <summary>
        /// Converts map element into an AnyValueMap or returns empty AnyValueMap if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>AnyValueMap value of the element or empty AnyValueMap if conversion is not supported.</returns>
        /// See <see cref="FromValue(object)"/>
        public AnyValueMap GetAsMap(string key)
        {
            var value = GetAsObject(key);
            return FromValue(value);
        }

        /// <summary>
        /// Converts map element into an AnyValueMap or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>AnyValueMap value of the element or default value if conversion is not supported.</returns>
        /// See <see cref="GetAsNullableMap(string)"/>
        public AnyValueMap GetAsMapWithDefault(string key, AnyValueMap defaultValue)
        {
            var result = GetAsNullableMap(key);
            return result ?? defaultValue;
        }

        /// <summary>
        /// Creates a binary clone of this object.
        /// </summary>
        /// <returns>a clone of this object.</returns>
        public object Clone()
        {
            return new AnyValueMap((IDictionary<string, object>)this);
        }

        /// <summary>
        /// Converts specified value into AnyValueMap.
        /// </summary>
        /// <param name="value">value to be converted</param>
        /// <returns>a newly created AnyValueMap.</returns>
        /// See <see cref="SetAsObject(object)"/>
        public static AnyValueMap FromValue(object value)
        {
            var result = new AnyValueMap();
            result.SetAsObject(value);
            return result;
        }

        /// <summary>
        /// Creates a new AnyValueMap from a list of key-value pairs called tuples.
        /// </summary>
        /// <param name="tuples">a list of values where odd elements are keys and the following
        /// even elements are values</param>
        /// <returns>a newly created AnyValueArray.</returns>
        public static AnyValueMap FromTuples(params object[] tuples)
        {
            var result = new AnyValueMap();
            if (tuples == null || tuples.Length == 0)
                return result;

            for (var index = 0; index < tuples.Length; index += 2)
            {
                if (index + 1 >= tuples.Length) break;

                var name = StringConverter.ToString(tuples[index]);
                var value = tuples[index + 1];

                result.SetAsObject(name, value);
            }

            return result;
        }

        /// <summary>
        /// Creates a new AnyValueMap by merging two or more maps. Maps defined later in
        /// the list override values from previously defined maps.
        /// </summary>
        /// <param name="maps">an array of maps to be merged</param>
        /// <returns>a newly created AnyValueMap.</returns>
        public static AnyValueMap FromMaps(params IDictionary[] maps)
        {
            var result = new AnyValueMap();
            if (maps != null && maps.Length > 0)
            {
                foreach (IDictionary map in maps)
                    result.Append(map);
            }
            return result;
        }
    }
}