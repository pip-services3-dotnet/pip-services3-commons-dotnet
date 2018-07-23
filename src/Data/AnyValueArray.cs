using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PipServices.Commons.Convert;

namespace PipServices.Commons.Data
{
    public class AnyValueArray : List<object>, ICloneable
    {
        public AnyValueArray()
        { }

        public AnyValueArray(object[] values)
        {
            Append(values);
        }

        public AnyValueArray(IEnumerable values)
        {
            Append(values);
        }

        public virtual object Get(int index)
        {
            return base[index];
        }

        public virtual void Set(int index, object value)
        {
            base[index] = value;
        }

        public object GetAsObject()
        {
            return new List<object>(this);
        }

        public void SetAsObject(object value)
        {
            Clear();
            Append(ArrayConverter.ToArray(value));
        }

        public void Append(object[] values)
        {
            AddRange(values);
        }

        public void Append(IEnumerable values)
        {
            foreach (var value in values)
                Add(value);
        }

        public object GetAsObject(int index)
        {
            return Get(index);
        }

        public void SetAsObject(int index, object value)
        {
            Set(index, value);
        }

        public string GetAsNullableString(int index)
        {
            return StringConverter.ToNullableString(this[index]);
        }

        public string GetAsString(int index)
        {
            return GetAsStringWithDefault(index, null);
        }

        public string GetAsStringWithDefault(int index, string defaultValue)
        {
            return StringConverter.ToStringWithDefault(this[index], defaultValue);
        }

        public bool? GetAsNullableBoolean(int index)
        {
            return BooleanConverter.ToNullableBoolean(this[index]);
        }

        public bool GetAsBoolean(int index)
        {
            return GetAsBooleanWithDefault(index, false);
        }

        public bool GetAsBooleanWithDefault(int index, bool defaultValue)
        {
            return BooleanConverter.ToBooleanWithDefault(this[index], defaultValue);
        }

        public int? GetAsNullableInteger(int index)
        {
            return IntegerConverter.ToNullableInteger(this[index]);
        }

        public int GetAsInteger(int index)
        {
            return GetAsIntegerWithDefault(index, 0);
        }

        public int GetAsIntegerWithDefault(int index, int defaultValue)
        {
            return IntegerConverter.ToIntegerWithDefault(this[index], defaultValue);
        }

        public long? GetAsNullableLong(int index)
        {
            return LongConverter.ToNullableLong(this[index]);
        }

        public long GetAsLong(int index)
        {
            return GetAsLongWithDefault(index, 0);
        }

        public long GetAsLongWithDefault(int index, long defaultValue)
        {
            return LongConverter.ToLongWithDefault(this[index], defaultValue);
        }

        public float? GetAsNullableFloat(int index)
        {
            return FloatConverter.ToNullableFloat(this[index]);
        }

        public float GetAsFloat(int index)
        {
            return GetAsFloatWithDefault(index, 0);
        }

        public float GetAsFloatWithDefault(int index, float defaultValue)
        {
            return FloatConverter.ToFloatWithDefault(this[index], defaultValue);
        }

        public double? GetAsNullableDouble(int index)
        {
            return DoubleConverter.ToNullableDouble(this[index]);
        }

        public double GetAsDouble(int index)
        {
            return GetAsDoubleWithDefault(index, 0);
        }

        public double GetAsDoubleWithDefault(int index, double defaultValue)
        {
            return DoubleConverter.ToDoubleWithDefault(this[index], defaultValue);
        }

        public DateTime? GetAsNullableDateTime(int index)
        {
            return DateTimeConverter.ToNullableDateTime(this[index]);
        }

        public DateTime GetAsDateTime(int index)
        {
            return GetAsDateTimeWithDefault(index, new DateTime());
        }

        public DateTime GetAsDateTimeWithDefault(int index, DateTime? defaultValue)
        {
            return DateTimeConverter.ToDateTimeWithDefault(this[index], defaultValue);
        }

        public TimeSpan? GetAsNullableTimeSpan(int index)
        {
            return TimeSpanConverter.ToNullableTimeSpan(this[index]);
        }

        public TimeSpan GetAsTimeSpan(int index)
        {
            return GetAsTimeSpanWithDefault(index, new TimeSpan(0));
        }

        public TimeSpan GetAsTimeSpanWithDefault(int index, TimeSpan? defaultValue)
        {
            return TimeSpanConverter.ToTimeSpanWithDefault(this[index], defaultValue);
        }

        public T? GetAsNullableEnum<T>(int index) where T : struct
        {
            return EnumConverter.ToNullableEnum<T>(this[index]);
        }

        public T GetAsEnum<T>(int index)
        {
            return GetAsEnumWithDefault<T>(index, default(T));
        }

        public T GetAsEnumWithDefault<T>(int index, T defaultValue)
        {
            return EnumConverter.ToEnumWithDefault<T>(this[index], defaultValue);
        }

        public T GetATypes<T>(int index)
        {

            return TypeConverter.ToType<T>(this[index]);
        }

        public T? GetAsNullableType<T>(int index) where T : struct
        {
            return TypeConverter.ToNullableType<T>(this[index]);
        }

        public T GetAsNullableTypeWithDefault<T>(int index, T defaultValue)
        {
            return TypeConverter.ToTypeWithDefault<T>(this[index], defaultValue);
        }

        public AnyValue GetAsValue(int index)
        {
            return new AnyValue(this[index]);
        }

        public AnyValueArray GetAsNullableArray(int index)
        {
            var value = GetAsObject(index);
            return value != null ? new AnyValueArray(new[] { value }) : null;
        }

        public AnyValueArray GetAsArray(int index)
        {
            return new AnyValueArray(new[] { this[index] });
        }

        public AnyValueArray GetAsArrayWithDefault(int index, AnyValueArray defaultValue)
        {
            var result = GetAsNullableArray(index);
            return result ?? defaultValue;
        }

        public AnyValueMap GetAsNullableMap(int index)
        {
            var value = GetAsObject(index);
            return value != null ? AnyValueMap.FromValue(value) : null;
        }

        public AnyValueMap GetAsMap(int index)
        {
            return AnyValueMap.FromValue(GetAsObject(index));
        }

        public AnyValueMap GetAsMapWithDefault(int index, AnyValueMap defaultValue)
        {
            var result = GetAsNullableMap(index);
            return result ?? defaultValue;
        }

        public new bool Contains(object value)
        {
            var strValue = StringConverter.ToNullableString(value);

            foreach (var thisValue in this)
            {
                var thisStrValue = StringConverter.ToNullableString(thisValue);
                if (strValue == thisStrValue)
                    return true;
            }

            return true;
        }

        public bool ContainsAs<T>(object value)
        {
            var typedValue = TypeConverter.ToType<T>(value);
            var strValue = StringConverter.ToNullableString(typedValue);

            foreach (var thisValue in this)
            {
                var thisTypedValue = StringConverter.ToNullableString(thisValue);
                var thisStrValue = StringConverter.ToNullableString(thisTypedValue);
                if (strValue == thisStrValue)
                    return true;
            }

            return true;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var index = 0; index < Count; index++)
            {
                if (index > 0)
                    builder.Append(',');
                builder.Append(StringConverter.ToStringWithDefault(base[index], ""));
            }
            return builder.ToString();
        }

        public object Clone()
        {
            return new AnyValueArray(this);
        }

        public static AnyValueArray FromValues(params object[] values)
        {
            return new AnyValueArray(values);
        }

        public static AnyValueArray FromValue(object value)
        {
            var values = ArrayConverter.ToNullableArray(value);
            return new AnyValueArray(values);
        }

        public static AnyValueArray FromString(string value, char separator, bool removeDuplicates)
        {
            var result = new AnyValueArray();
            HashSet<string> hash = null;

            if (string.IsNullOrEmpty(value))
                return result;

            var items = value.Split(separator);
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (removeDuplicates)
                    {
                        if (hash == null) hash = new HashSet<string>();
                        if (hash.Contains(item)) continue;
                        hash.Add(item);
                    }
                    result.Add(item != null ? new AnyValue(item) : null);
                }
            }

            return result;
        }

        public static AnyValueArray FromString(string value, char separator)
        {
            return FromString(value, separator, false);
        }
    }
}