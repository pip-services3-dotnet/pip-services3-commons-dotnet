using PipServices3.Commons.Convert;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Cross-language implementation of a map (dictionary) where all keys and values are strings.
    /// The stored values can be converted to different types using variety of accessor methods.
    /// 
    /// The string map is highly versatile.It can be converted into many formats, stored and 
    /// sent over the wire.
    /// 
    /// This class is widely used in Pip.Services as a basis for variety of classes, such as 
    /// <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-commons-dotnet/master/doc/api/class_pip_services_1_1_commons_1_1_config_1_1_config_params.html">ConfigParams</a>, 
    /// <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-components-dotnet/master/doc/api/class_pip_services_1_1_components_1_1_connect_1_1_connection_params.html">ConnectionParams</a>, 
    /// <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-components-dotnet/master/doc/api/class_pip_services_1_1_components_1_1_auth_1_1_credential_params.html">CredentialParams</a> and others.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = StringValueMap.FromString("key1=1;key2=123.456;key3=2018-01-01");
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
    public class StringValueMap : Dictionary<string, string>
    {
        /// <summary>
        /// Creates a new instance of the map and assigns its value.
        /// </summary>
        public StringValueMap()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Creates a new instance of the map and assigns its value.
        /// </summary>
        /// <param name="map">(optional) values to initialize this map.</param>
        public StringValueMap(IDictionary<string, string> map)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            Append(map);
        }

        /// <summary>
        /// Creates a new instance of the map and assigns its value.
        /// </summary>
        /// <param name="value">(optional) values to initialize this map.</param>
        public StringValueMap(object value)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            Append(MapConverter.ToMap(value));
        }

        public new string this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        /// <summary>
        /// Gets a map element specified by its key.
        /// </summary>
        /// <param name="key">a key of the element to get.</param>
        /// <returns>the value of the map element.</returns>
        public virtual string Get(string key)
        {
            string value = null;
            base.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Sets a new value for this array element by its key
        /// </summary>
        /// <param name="key">a key of the element to set.</param>
        /// <param name="value">the new object value.</param>
        public virtual void Set(string key, string value)
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
                SetAsObject(key, StringConverter.ToNullableString(map[key]));
        }

        /// <summary>
        /// Appends new elements to this map.
        /// </summary>
        /// <param name="map">a map with elements to be added.</param>
        public void Append(IDictionary<string, string> map)
        {
            if (map == null || map.Count == 0) return;

            foreach (var key in map.Keys)
                SetAsObject(key, map[key]);
        }

        /// <summary>
        /// Gets the value stored in this map element without any conversions
        /// </summary>
        /// <returns>the value of the map element.</returns>
        public object GetAsObject()
        {
            var result = new Dictionary<string, object>();
            foreach (var key in Keys)
                result[key] = this[key];
            return result;
        }

        /// <summary>
        /// Sets a new value for this array element
        /// </summary>
        /// <param name="value">the new object value.</param>
        public void SetAsObject(object value)
        {
            Clear();
            Append(MapConverter.ToMap(value));
        }

        /// <summary>
        /// Sets a new value to map element specified by its index. When the index is not defined, 
        /// it resets the entire map value.This method has double purpose
        /// because method overrides are not supported in JavaScript.
        /// </summary>
        /// <param name="key">(optional) a key of the element to set</param>
        /// <param name="value">a new element or map value.</param>
        public void SetAsObject(string key, object value)
        {
            Set(key, StringConverter.ToNullableString(value));
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
        /// is not possible it returns null.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="key">a key of element to get.</param>
        /// <returns>element value defined by the typecode or null if conversion is not supported.</returns>
        /// See <see cref="TypeConverter.ToNullableType{T}(object)"/>
        public T? GetAsNullableType<T>(string key) where T : struct
        {
            return TypeConverter.ToNullableType<T>(key);
        }

        /// <summary>
        /// Converts map element into a value defined by specied typecode. If conversion 
        /// is not possible it returns default value for the specied typecode.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="key">a key of element to get.</param>
        /// <returns>element value defined by the typecode or default value if conversion is not supported.</returns>
        /// See <see cref="TypeConverter.ToType{T}(object)"/>
        public T GetAsType<T>(string key) where T : struct
        {
            return TypeConverter.ToType<T>(key);
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
        public T GetAsTypeWithDefault<T>(string key, T defaultValue) where T : struct
        {
            return TypeConverter.ToTypeWithDefault<T>(key, defaultValue);
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
            return AnyValueArray.FromValue(GetAsObject(key));
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
            var value = GetAsNullableArray(key);
            return value != null ? value : defaultValue;
        }

        /// <summary>
        /// Converts map element into an AnyValueMap or returns null if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>AnyValueMap value of the element or null if conversion is not supported.</returns>
        /// See <see cref="FromValue(object)"/>
        public AnyValueMap GetAsNullableMap(string key)
        {
            var result = GetAsObject(key);
            return result != null ? AnyValueMap.FromValue(result) : null;
        }

        /// <summary>
        /// Converts map element into an AnyValueMap or returns empty AnyValueMap if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>AnyValueMap value of the element or empty AnyValueMap if conversion is not supported.</returns>
        /// See <see cref="FromValue(object)"/>
        public AnyValueMap GetAsMap(string key)
        {
            return AnyValueMap.FromValue(GetAsObject(key));
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
        /// Gets a string representation of the object. The result is a
        /// semicolon-separated list of key-value pairs as "key1=value1;key2=value2;key=value3"
        /// </summary>
        /// <returns>a string representation of the object.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var key in Keys)
            {
                if (builder.Length > 0)
                    builder.Append(";");

                var value = this[key];
                if (value != null)
                    builder.Append(key + "=" + value);
                else
                    builder.Append(key);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Creates a binary clone of this object.
        /// </summary>
        /// <returns>a clone of this object.</returns>
        public object Clone()
        {
            return new StringValueMap(this);
        }

        /// <summary>
        /// Converts specified value into StringValueMap.
        /// </summary>
        /// <param name="value">value to be converted</param>
        /// <returns>a newly created StringValueMap.</returns>
        public static StringValueMap FromValue(object value)
        {
            return new StringValueMap(value);
        }

        /// <summary>
        /// Creates a new StringValueMap from a list of key-value pairs called tuples.
        /// </summary>
        /// <param name="tuples">a list of values where odd elements are keys and the following
        /// even elements are values</param>
        /// <returns>a newly created StringValueArray.</returns>
        public static StringValueMap FromTuples(params object[] tuples)
        {
            var result = new StringValueMap();

            if (tuples == null || tuples.Length == 0)
                return result;

            for (var i = 0; i < tuples.Length; i += 2)
            {
                if (i + 1 >= tuples.Length) break;

                var name = StringConverter.ToString(tuples[i]);
                var value = StringConverter.ToString(tuples[i + 1]);

                result[name] = value;
            }

            return result;
        }

        /// <summary>
        /// arses semicolon-separated key-value pairs and returns them as a StringValueMap.
        /// </summary>
        /// <param name="line">semicolon-separated key-value list to initialize StringValueMap.</param>
        /// <returns>a newly created StringValueMap.</returns>
        public static StringValueMap FromString(string line)
        {
            var result = new StringValueMap();
            if (string.IsNullOrWhiteSpace(line))
                return result;

            var tokens = line.Split(';');
            foreach (var token in tokens)
            {
                if (token.Length == 0) continue;
                var index = token.IndexOf("=");
                var key = index > 0 ? token.Substring(0, index).Trim() : token.Trim();
                var value = index > 0 ? token.Substring(index + 1).Trim() : null;
                result[key] = value;
            }
            return result;
        }

        /// <summary>
        /// Creates a new StringValueMap by merging two or more maps. Maps defined later in
        /// the list override values from previously defined maps.
        /// </summary>
        /// <param name="maps">an array of maps to be merged</param>
        /// <returns>a newly created StringValueMap.</returns>
        public static StringValueMap FromMaps(params IDictionary<string, string>[] maps)
        {
            var result = new StringValueMap();
            if (maps != null && maps.Length > 0)
            {
                foreach (var map in maps)
                    result.Append(map);
            }
            return result;
        }
    }
}