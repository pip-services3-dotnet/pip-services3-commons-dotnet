using System;
using Newtonsoft.Json;
using PipServices.Commons.Convert;

namespace PipServices.Commons.Data
{
    public class AnyValue : ICloneable
    {
        public AnyValue(object value = null)
        {
            var anyValue = value as AnyValue;

            Value = anyValue != null ? anyValue.Value : value;
        }

        public AnyValue(AnyValue value)
        {
            Value = value.Value;
        }

        [JsonProperty("value")]
        public object Value { get; private set; }

        public object GetAsObject()
        {
            return Value;
        }

        public void SetAsObject(object value)
        {
            Value = value;
        }

        public string GetAsNullableString()
        {
            return StringConverter.ToNullableString(Value);
        }

        public string GetAsString()
        {
            return GetAsStringWithDefault(null);
        }

        public string GetAsStringWithDefault(string defaultValue)
        {
            return StringConverter.ToStringWithDefault(Value, defaultValue);
        }

        public bool? GetAsNullableBoolean()
        {
            return BooleanConverter.ToNullableBoolean(Value);
        }

        public bool GetAsBoolean()
        {
            return GetAsBooleanWithDefault(false);
        }

        public bool GetAsBooleanWithDefault(bool defaultValue)
        {
            return BooleanConverter.ToBooleanWithDefault(Value, defaultValue);
        }

        public int? GetAsNullableInteger()
        {
            return IntegerConverter.ToNullableInteger(Value);
        }

        public int GetAsInteger()
        {
            return GetAsIntegerWithDefault(0);
        }

        public int GetAsIntegerWithDefault(int defaultValue)
        {
            return IntegerConverter.ToIntegerWithDefault(Value, defaultValue);
        }

        public long? GetAsNullableLong()
        {
            return LongConverter.ToNullableLong(Value);
        }

        public long GetAsLong()
        {
            return GetAsLongWithDefault(0);
        }

        public long GetAsLongWithDefault(long defaultValue)
        {
            return LongConverter.ToLongWithDefault(Value, defaultValue);
        }

        public float? GetAsNullableFloat()
        {
            return FloatConverter.ToNullableFloat(Value);
        }

        public float GetAsFloat()
        {
            return GetAsFloatWithDefault(0);
        }

        public float GetAsFloatWithDefault(float defaultValue)
        {
            return FloatConverter.ToFloatWithDefault(Value, defaultValue);
        }

        public double? GetAsNullableDouble()
        {
            return DoubleConverter.ToNullableDouble(Value);
        }

        public double GetAsDouble()
        {
            return GetAsDoubleWithDefault(0);
        }

        public double GetAsDoubleWithDefault(double defaultValue)
        {
            return DoubleConverter.ToDoubleWithDefault(Value, defaultValue);
        }

        public DateTime? GetAsNullableDateTime()
        {
            return DateTimeConverter.ToNullableDateTime(Value);
        }

        public DateTime GetAsDateTime()
        {
            return GetAsDateTimeWithDefault(new DateTime());
        }

        public DateTime GetAsDateTimeWithDefault(DateTime? defaultValue)
        {
            return DateTimeConverter.ToDateTimeWithDefault(Value, defaultValue);
        }

        public TimeSpan? GetAsNullableTimeSpan()
        {
            return TimeSpanConverter.ToNullableTimeSpan(Value);
        }

        public TimeSpan GetAsTimeSpan()
        {
            return GetAsTimeSpanWithDefault(new TimeSpan(0));
        }

        public TimeSpan GetAsTimeSpanWithDefault(TimeSpan? defaultValue)
        {
            return TimeSpanConverter.ToTimeSpanWithDefault(Value, defaultValue);
        }

        public T? GetAsNullableEnum<T>() where T : struct
        {
            return EnumConverter.ToNullableEnum<T>(Value);
        }

        public T GetAsEnum<T>()
        {
            return GetAsEnumWithDefault<T>(default(T));
        }

        public T GetAsEnumWithDefault<T>(T defaultValue)
        {
            return EnumConverter.ToEnumWithDefault<T>(Value, defaultValue);
        }

        public T? GetAsNullableType<T>() where T : struct
        {
            return TypeConverter.ToNullableType<T>(Value);
        }

        public T GetAsType<T>() where T : struct
        {
            return TypeConverter.ToType<T>(Value);
        }

        public T GetAsTypeWithDefault<T>(T defaultValue) where T : struct
        {
            return TypeConverter.ToTypeWithDefault<T>(Value, defaultValue);
        }

        public AnyValueArray GetAsArray()
        {
            return AnyValueArray.FromValue(Value);
        }

        public AnyValueMap GetAsMap()
        {
            return AnyValueMap.FromValue(Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null && Value == null) return true;
            if (obj == null || Value == null) return false;

            obj = obj is AnyValue ? ((AnyValue)obj).Value : obj;

            return StringConverter.ToString(Value) == StringConverter.ToString(obj);
        }

        public bool EqualsAs<T>(object obj)
        {
            if (obj == null && Value == null) return true;
            if (obj == null || Value == null) return false;

            obj = obj is AnyValue ? ((AnyValue)obj).Value : obj;

            var value1 = TypeConverter.ToType<T>(Value);
            var value2 = TypeConverter.ToType<T>(obj);

            if (value1 == null && value2 == null) return true;
            if (value1 == null || Value == null) return false;

            return StringConverter.ToString(value1) == StringConverter.ToString(value2);
        }

        public override string ToString()
        {
            return StringConverter.ToString(Value);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Value?.GetHashCode() ?? 0;
        }

        public object Clone()
        {
            return new AnyValue(Value);
        }
    }
}