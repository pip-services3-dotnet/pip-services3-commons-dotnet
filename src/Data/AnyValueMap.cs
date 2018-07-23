using PipServices.Commons.Convert;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PipServices.Commons.Data
{
    public class AnyValueMap : Dictionary<string, object>, ICloneable
    {
        public AnyValueMap()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public AnyValueMap(IDictionary<string, object> values)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            Append(values);
        }

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

        public virtual object Get(string key)
        {
            object value = null;
            base.TryGetValue(key, out value);
            return value;
        }

        public virtual void Set(string key, object value)
        {
            base[key] = value;
        }

        public void Append(IDictionary<string, object> map)
        {
            if (map == null || map.Count == 0) return;

            foreach (var key in map.Keys)
                Set(key, map[key]);
        }

        public void Append(IDictionary map)
        {
            if (map == null || map.Count == 0) return;

            foreach (var key in map.Keys)
                Set(key.ToString(), map[key]);
        }

        public object GetAsObject()
        {
            return new Dictionary<string, object>(this);
        }

        public object GetAsObject(string key)
        {
            return Get(key);
        }

        public void SetAsObject(object value)
        {
            Clear();
            Append((IDictionary)MapConverter.ToMap(value));
        }

        public void SetAsObject(string key, object value)
        {
            Set(key, value);
        }

        public string GetAsNullableString(string key)
        {
            var value = Get(key);
            return StringConverter.ToNullableString(value);
        }

        public string GetAsString(string key)
        {
            return GetAsStringWithDefault(key, null);
        }

        public string GetAsStringWithDefault(string key, string defaultValue)
        {
            var value = Get(key);
            return StringConverter.ToStringWithDefault(value, defaultValue);
        }

        public bool? GetAsNullableBoolean(string key)
        {
            var value = Get(key);
            return BooleanConverter.ToNullableBoolean(value);
        }

        public bool GetAsBoolean(string key)
        {
            return GetAsBooleanWithDefault(key, false);
        }

        public bool GetAsBooleanWithDefault(string key, bool defaultValue)
        {
            var value = Get(key);
            return BooleanConverter.ToBooleanWithDefault(value, defaultValue);
        }

        public int? GetAsNullableInteger(string key)
        {
            var value = Get(key);
            return IntegerConverter.ToNullableInteger(value);
        }

        public int GetAsInteger(string key)
        {
            return GetAsIntegerWithDefault(key, 0);
        }

        public int GetAsIntegerWithDefault(string key, int defaultValue)
        {
            var value = Get(key);
            return IntegerConverter.ToIntegerWithDefault(value, defaultValue);
        }

        public long? GetAsNullableLong(string key)
        {
            var value = Get(key);
            return LongConverter.ToNullableLong(value);
        }

        public long GetAsLong(string key)
        {
            return GetAsLongWithDefault(key, 0);
        }

        public long GetAsLongWithDefault(string key, long defaultValue)
        {
            var value = Get(key);
            return LongConverter.ToLongWithDefault(value, defaultValue);
        }

        public float? GetAsNullableFloat(string key)
        {
            var value = Get(key);
            return FloatConverter.ToNullableFloat(value);
        }

        public float GetAsFloat(string key)
        {
            return GetAsFloatWithDefault(key, 0);
        }

        public float GetAsFloatWithDefault(string key, float defaultValue)
        {
            var value = Get(key);
            return FloatConverter.ToFloatWithDefault(value, defaultValue);
        }

        public double? GetAsNullableDouble(string key)
        {
            var value = Get(key);
            return DoubleConverter.ToNullableDouble(value);
        }

        public double GetAsDouble(string key)
        {
            return GetAsDoubleWithDefault(key, 0);
        }

        public double GetAsDoubleWithDefault(string key, double defaultValue)
        {
            var value = Get(key);
            return DoubleConverter.ToDoubleWithDefault(value, defaultValue);
        }

        public DateTime? GetAsNullableDateTime(string key)
        {
            var value = Get(key);
            return DateTimeConverter.ToNullableDateTime(value);
        }

        public DateTime GetAsDateTime(string key)
        {
            return GetAsDateTimeWithDefault(key, new DateTime());
        }

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

        public T GetAsType<T>(string key)
        {
            var value = Get(key);
            return TypeConverter.ToType<T>(value);
        }

        public T? GetAsNullableType<T>(string key) where T : struct
        {
            var value = Get(key);
            return TypeConverter.ToNullableType<T>(value);
        }

        public T GetAsTypeWithDefault<T>(string key, T defaultValue)
        {
            var value = Get(key);
            return TypeConverter.ToTypeWithDefault<T>(value, defaultValue);
        }

        public AnyValue GetAsValue(string key)
        {
            return new AnyValue(GetAsObject(key));
        }

        public AnyValueArray GetAsNullableArray(string key)
        {
            var value = GetAsObject(key);
            return value != null ? AnyValueArray.FromValue(value) : null;
        }

        public AnyValueArray GetAsArray(string key)
        {
            var value = GetAsObject(key);
            return AnyValueArray.FromValue(key);
        }

        public AnyValueArray GetAsArrayWithDefault(string key, AnyValueArray defaultValue)
        {
            var result = GetAsNullableArray(key);
            return result ?? defaultValue;
        }

        public AnyValueMap GetAsNullableMap(string key)
        {
            var value = GetAsObject(key);
            return value != null ? FromValue(value) : null;
        }

        public AnyValueMap GetAsMap(string key)
        {
            var value = GetAsObject(key);
            return FromValue(value);
        }

        public AnyValueMap GetAsMapWithDefault(string key, AnyValueMap defaultValue)
        {
            var result = GetAsNullableMap(key);
            return result ?? defaultValue;
        }

        public object Clone()
        {
            return new AnyValueMap((IDictionary<string, object>)this);
        }

        public static AnyValueMap FromValue(object value)
        {
            var result = new AnyValueMap();
            result.SetAsObject(value);
            return result;
        }

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