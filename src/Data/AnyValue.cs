using System;
using Newtonsoft.Json;
using PipServices3.Commons.Convert;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Cross-language implementation of dynamic object what can hold value of any type.
    /// The stored value can be converted to different types using variety of accessor methods.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = new AnyValue("123.456");
    /// 
    /// value1.GetAsInteger();   // Result: 123
    /// value1.GetAsString();    // Result: "123.456"
    /// value1.GetAsFloat();     // Result: 123.456
    /// </code>
    /// </example>
    /// See <see cref="StringConverter"/>, <see cref="BooleanConverter"/>, <see cref="IntegerConverter"/>, 
    /// <see cref="LongConverter"/>, <see cref="DoubleConverter"/>, <see cref="FloatConverter"/>, 
    /// <see cref="DateTimeConverter"/>, <see cref="ICloneable"/>
    public class AnyValue : ICloneable
    {
        /// <summary>
        /// Creates a new instance of the object and assigns its value.
        /// </summary>
        /// <param name="value">(optional) value to initialize this object.</param>
        public AnyValue(object value = null)
        {
            var anyValue = value as AnyValue;

            Value = anyValue != null ? anyValue.Value : value;
        }

        /// <summary>
        /// Creates a new instance of the object and assigns its value.
        /// </summary>
        /// <param name="value">(optional) value to initialize this object.</param>
        public AnyValue(AnyValue value)
        {
            Value = value.Value;
        }

        /// <summary>
        /// The value stored by this object.
        /// </summary>
        [JsonProperty("value")]
        public object Value { get; private set; }

        /// <summary>
        /// Gets the value stored in this object without any conversions
        /// </summary>
        /// <returns>the object value.</returns>
        public object GetAsObject()
        {
            return Value;
        }

        /// <summary>
        /// Sets a new value for this object
        /// </summary>
        /// <param name="value">the new object value.</param>
        public void SetAsObject(object value)
        {
            if (value is AnyValue)
                Value = ((AnyValue)value).Value;
            else
                Value = value;
        }

        /// <summary>
        /// Converts object value into a string or returns null if conversion is not possible.
        /// </summary>
        /// <returns>string value or null if conversion is not supported.</returns>
        /// See <see cref="StringConverter.ToNullableString(object)"/>
        public string GetAsNullableString()
        {
            return StringConverter.ToNullableString(Value);
        }

        /// <summary>
        /// Converts object value into a string or returns "" if conversion is not possible.
        /// </summary>
        /// <returns>string value or "" if conversion is not supported.</returns>
        /// See <see cref="GetAsStringWithDefault(string)"/>
        public string GetAsString()
        {
            return GetAsStringWithDefault(null);
        }

        /// <summary>
        /// Converts object value into a string or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="defaultValue">the default value.</param>
        /// <returns>string value or default if conversion is not supported.</returns>
        /// See <see cref="StringConverter.ToStringWithDefault(object, string)"/>
        public string GetAsStringWithDefault(string defaultValue)
        {
            return StringConverter.ToStringWithDefault(Value, defaultValue);
        }

        /// <summary>
        /// Converts object value into a boolean or returns null if conversion is not possible.
        /// </summary>
        /// <returns>boolean value or null if conversion is not supported.</returns>
        /// See <see cref="BooleanConverter.ToNullableBoolean(object)"/>
        public bool? GetAsNullableBoolean()
        {
            return BooleanConverter.ToNullableBoolean(Value);
        }

        /// <summary>
        /// Converts object value into a boolean or returns false if conversion is not possible.
        /// </summary>
        /// <returns>string value or false if conversion is not supported.</returns>
        /// See <see cref="GetAsBooleanWithDefault(bool)"/>
        public bool GetAsBoolean()
        {
            return GetAsBooleanWithDefault(false);
        }

        /// <summary>
        /// Converts object value into a boolean or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="defaultValue">the default value</param>
        /// <returns>boolean value or default if conversion is not supported.</returns>
        /// See <see cref="BooleanConverter.ToBooleanWithDefault(object, bool)"/>
        public bool GetAsBooleanWithDefault(bool defaultValue)
        {
            return BooleanConverter.ToBooleanWithDefault(Value, defaultValue);
        }

        /// <summary>
        /// Converts object value into an integer or returns null if conversion is not possible.
        /// </summary>
        /// <returns>integer value or null if conversion is not supported.</returns>
        /// See <see cref="IntegerConverter.ToNullableInteger(object)"/>
        public int? GetAsNullableInteger()
        {
            return IntegerConverter.ToNullableInteger(Value);
        }

        /// <summary>
        /// onverts object value into an integer or returns 0 if conversion is not possible.
        /// </summary>
        /// <returns>integer value or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsIntegerWithDefault(int)"/>
        public int GetAsInteger()
        {
            return GetAsIntegerWithDefault(0);
        }

        /// <summary>
        /// Converts object value into a integer or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="defaultValue">the default value</param>
        /// <returns>integer value or default if conversion is not supported.</returns>
        public int GetAsIntegerWithDefault(int defaultValue)
        {
            return IntegerConverter.ToIntegerWithDefault(Value, defaultValue);
        }

        /// <summary>
        /// Converts object value into a long or returns null if conversion is not possible.
        /// </summary>
        /// <returns>long value or null if conversion is not supported.</returns>
        /// See <see cref="LongConverter.ToNullableLong(object)"/>
        public long? GetAsNullableLong()
        {
            return LongConverter.ToNullableLong(Value);
        }

        /// <summary>
        /// Converts object value into a long or returns 0 if conversion is not possible.
        /// </summary>
        /// <returns>string value or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsLongWithDefault(long)"/>
        public long GetAsLong()
        {
            return GetAsLongWithDefault(0);
        }

        /// <summary>
        /// Converts object value into a long or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="defaultValue">the default value</param>
        /// <returns>long value or default if conversion is not supported.</returns>
        /// See <see cref="LongConverter.ToLongWithDefault(object, long)"/>
        public long GetAsLongWithDefault(long defaultValue)
        {
            return LongConverter.ToLongWithDefault(Value, defaultValue);
        }

        /// <summary>
        /// Converts object value into a float or returns null if conversion is not possible.
        /// </summary>
        /// <returns>float value or null if conversion is not supported.</returns>
        /// See <see cref="FloatConverter.ToNullableFloat(object)"/>
        public float? GetAsNullableFloat()
        {
            return FloatConverter.ToNullableFloat(Value);
        }

        /// <summary>
        /// Converts object value into a float or returns 0 if conversion is not possible.
        /// </summary>
        /// <returns>float value or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsFloatWithDefault(float)"/>
        public float GetAsFloat()
        {
            return GetAsFloatWithDefault(0);
        }

        /// <summary>
        /// Converts object value into a float or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="defaultValue">the default value</param>
        /// <returns>float value or default if conversion is not supported.</returns>
        /// See <see cref="FloatConverter.ToFloatWithDefault(object, float)"/>
        public float GetAsFloatWithDefault(float defaultValue)
        {
            return FloatConverter.ToFloatWithDefault(Value, defaultValue);
        }

        /// <summary>
        /// Converts object value into a double or returns null if conversion is not possible.
        /// </summary>
        /// <returns>double value or null if conversion is not supported.</returns>
        /// See <see cref="DoubleConverter.ToNullableDouble(object)"/>
        public double? GetAsNullableDouble()
        {
            return DoubleConverter.ToNullableDouble(Value);
        }

        /// <summary>
        /// Converts object value into a double or returns 0 if conversion is not possible.
        /// </summary>
        /// <returns>double value or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsDoubleWithDefault(double)"/>
        public double GetAsDouble()
        {
            return GetAsDoubleWithDefault(0);
        }

        /// <summary>
        /// Converts object value into a double or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="defaultValue">the default value</param>
        /// <returns>double value or default if conversion is not supported.</returns>
        /// See <see cref="DoubleConverter.ToDoubleWithDefault(object, double)"/>
        public double GetAsDoubleWithDefault(double defaultValue)
        {
            return DoubleConverter.ToDoubleWithDefault(Value, defaultValue);
        }

        /// <summary>
        /// Converts object value into a Date or returns null if conversion is not possible.
        /// </summary>
        /// <returns>DateTime value or null if conversion is not supported.</returns>
        /// See <see cref="DateTimeConverter.ToNullableDateTime(object)"/>
        public DateTime? GetAsNullableDateTime()
        {
            return DateTimeConverter.ToNullableDateTime(Value);
        }

        /// <summary>
        /// Converts object value into a Date or returns current date if conversion is not possible.
        /// </summary>
        /// <returns>DateTime value or current date if conversion is not supported.</returns>
        /// See <see cref="GetAsDateTimeWithDefault(DateTime?)"/>
        public DateTime GetAsDateTime()
        {
            return GetAsDateTimeWithDefault(new DateTime());
        }

        /// <summary>
        /// Converts object value into a Date or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="defaultValue">the default value</param>
        /// <returns>DateTime value or default if conversion is not supported.</returns>
        /// See <see cref="DateTimeConverter.ToDateTimeWithDefault(object, DateTime?)"/>
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

        /// <summary>
        /// Converts object value into a value defined by specied typecode. If conversion is not possible it returns null.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <returns>value defined by the typecode or null if conversion is not supported.</returns>
        /// See <see cref="TypeConverter.ToNullableType{T}(object)"/>
        public T? GetAsNullableType<T>() where T : struct
        {
            return TypeConverter.ToNullableType<T>(Value);
        }

        /// <summary>
        /// Converts object value into a value defined by specied typecode. If conversion
        /// is not possible it returns default value for the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <returns>value defined by the typecode or type default value if conversion is
        /// not supported.</returns>
        /// See <see cref="TypeConverter.ToType{T}(object)"/>
        public T GetAsType<T>() where T : struct
        {
            return TypeConverter.ToType<T>(Value);
        }

        /// <summary>
        /// Converts object value into a value defined by specied typecode. If conversion
        /// is not possible it returns default value.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="defaultValue">the default value</param>
        /// <returns>value defined by the typecode or type default value if conversion is not supported.</returns>
        public T GetAsTypeWithDefault<T>(T defaultValue) where T : struct
        {
            return TypeConverter.ToTypeWithDefault<T>(Value, defaultValue);
        }
        /// <summary>
        /// Converts object value into an AnyArray or returns empty AnyArray if conversion is not possible.
        /// </summary>
        /// <returns>AnyArray value or empty AnyArray if conversion is not supported.</returns>
        /// See <see cref="AnyValueArray.FromValue(object)"/>
        public AnyValueArray GetAsArray()
        {
            return AnyValueArray.FromValue(Value);
        }

        /// <summary>
        /// Converts object value into AnyMap or returns empty AnyMap if conversion is not possible.
        /// </summary>
        /// <returns>AnyMap value or empty AnyMap if conversion is not supported.</returns>
        /// See <see cref="AnyValueMap.FromValue(object)"/>
        public AnyValueMap GetAsMap()
        {
            return AnyValueMap.FromValue(Value);
        }

        /// <summary>
        /// Compares this object value to specified specified value. When direct
        /// comparison gives negative results it tries to compare values as strings.
        /// </summary>
        /// <param name="obj">the value to be compared with.</param>
        /// <returns>true when objects are equal and false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null && Value == null) return true;
            if (obj == null || Value == null) return false;

            obj = obj is AnyValue ? ((AnyValue)obj).Value : obj;

            return StringConverter.ToString(Value) == StringConverter.ToString(obj);
        }

        /// <summary>
        /// ompares this object value to specified specified value. When direct
        /// comparison gives negative results it converts values to type specified by
        /// type code and compare them again.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="obj">the value to be compared with.</param>
        /// <returns>true when objects are equal and false otherwise.</returns>
        /// See <see cref="TypeConverter.ToType{T}(object)"/>
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

        /// <summary>
        /// Gets a string representation of the object.
        /// </summary>
        /// <returns>a string representation of the object.</returns>
        /// See <see cref="StringConverter.ToString(object)"/>
        public override string ToString()
        {
            return StringConverter.ToString(Value);
        }

        /// <summary>
        /// Gets an object hash code which can be used to optimize storing and searching.
        /// </summary>
        /// <returns>an object hash code.</returns>
        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Value?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Creates a binary clone of this object.
        /// </summary>
        /// <returns>a clone of this object.</returns>
        public object Clone()
        {
            return new AnyValue(Value);
        }
    }
}