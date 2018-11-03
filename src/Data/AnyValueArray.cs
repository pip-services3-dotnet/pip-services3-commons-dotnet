using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PipServices3.Commons.Convert;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Cross-language implementation of dynamic object array what can hold values of any type.
    /// The stored values can be converted to different types using variety of accessor methods.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = new AnyValueArray(new object[]{1, "123.456", "2018-01-01"});
    /// 
    /// value1.GetAsBoolean(0);   // Result: true
    /// value1.GetAsInteger(1);   // Result: 123
    /// value1.GetAsFloat(1);     // Result: 123.456
    /// value1.GetAsDateTime(2);  // Result: new Date(2018,0,1)
    /// </code>
    /// </example>
    /// See <see cref="StringConverter"/>, <see cref="BooleanConverter"/>, <see cref="IntegerConverter"/>, 
    /// <see cref="LongConverter"/>, <see cref="DoubleConverter"/>, <see cref="FloatConverter"/>, 
    /// <see cref="DateTimeConverter"/>, <see cref="ICloneable"/>
    public class AnyValueArray : List<object>, ICloneable
    {
        /// <summary>
        /// Creates a new instance of the array.
        /// </summary>
        public AnyValueArray()
        { }

        /// <summary>
        /// Creates a new instance of the array and assigns its value.
        /// </summary>
        /// <param name="values">(optional) values to initialize this array.</param>
        public AnyValueArray(object[] values)
        {
            Append(values);
        }

        /// <summary>
        ///  Creates a new instance of the array and assigns its value.
        /// </summary>
        /// <param name="values">(optional) values to initialize this array.</param>
        public AnyValueArray(IEnumerable values)
        {
            Append(values);
        }

        /// <summary>
        /// Gets an array element specified by its index.
        /// </summary>
        /// <param name="index">an index of the element to get.</param>
        /// <returns>the value of the array element.</returns>
        public virtual object Get(int index)
        {
            return base[index];
        }

        /// <summary>
        /// Sets a new value into array element specified by its index.
        /// </summary>
        /// <param name="index">an index of the element to put.</param>
        /// <param name="value">a new value for array element.</param>
        public virtual void Set(int index, object value)
        {
            base[index] = value;
        }

        /// <summary>
        /// Gets the value stored in this array element without any conversions
        /// </summary>
        /// <returns>the value of the array element.</returns>
        public object GetAsObject()
        {
            return new List<object>(this);
        }

        /// <summary>
        /// Sets a new value for this array element
        /// </summary>
        /// <param name="value">the new object value.</param>
        public void SetAsObject(object value)
        {
            Clear();
            Append(ArrayConverter.ToArray(value));
        }

        /// <summary>
        /// Appends new elements to this array.
        /// </summary>
        /// <param name="values">a list of elements to be added.</param>
        public void Append(object[] values)
        {
            AddRange(values);
        }

        /// <summary>
        /// Appends new elements to this array.
        /// </summary>
        /// <param name="values">a list of elements to be added.</param>
        public void Append(IEnumerable values)
        {
            foreach (var value in values)
                Add(value);
        }

        /// <summary>
        /// Gets an array element specified by its index.
        /// </summary>
        /// <param name="index">an index of the element to get.</param>
        /// <returns>the value of the array element.</returns>
        public object GetAsObject(int index)
        {
            return Get(index);
        }

        /// <summary>
        /// Sets a new value into array element specified by its index.
        /// </summary>
        /// <param name="index">an index of the element to put.</param>
        /// <param name="value">a new value for array element.</param>
        public void SetAsObject(int index, object value)
        {
            Set(index, value);
        }

        /// <summary>
        /// Converts array element into a string or returns null if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>string value of the element or null if conversion is not supported.</returns>
        /// See <see cref="StringConverter.ToNullableString(object)"/>
        public string GetAsNullableString(int index)
        {
            return StringConverter.ToNullableString(this[index]);
        }

        /// <summary>
        /// Converts array element into a string or returns "" if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>string value of the element or "" if conversion is not supported.</returns>
        /// See <see cref="GetAsStringWithDefault(int, string)"/>
        public string GetAsString(int index)
        {
            return GetAsStringWithDefault(index, null);
        }

        /// <summary>
        /// Converts array element into a string or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>string value of the element or default value if conversion is not supported.</returns>
        /// See <see cref="StringConverter.ToStringWithDefault(object, string)"/>
        public string GetAsStringWithDefault(int index, string defaultValue)
        {
            return StringConverter.ToStringWithDefault(this[index], defaultValue);
        }

        /// <summary>
        /// Converts array element into a boolean or returns null if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>boolean of the element or null if conversion is not supported.</returns>
        /// See <see cref="BooleanConverter.ToNullableBoolean(object)"/>
        public bool? GetAsNullableBoolean(int index)
        {
            return BooleanConverter.ToNullableBoolean(this[index]);
        }

        /// <summary>
        /// Converts array element into a boolean or returns false if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>boolean value of the element or false if conversion is not supported.</returns>
        /// See <see cref="GetAsBooleanWithDefault(int, bool)"/>
        public bool GetAsBoolean(int index)
        {
            return GetAsBooleanWithDefault(index, false);
        }

        /// <summary>
        /// Converts array element into a boolean or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>boolean of the element or default value if conversion is not supported.</returns>
        /// See <see cref="BooleanConverter.ToBooleanWithDefault(object, bool)"/>
        public bool GetAsBooleanWithDefault(int index, bool defaultValue)
        {
            return BooleanConverter.ToBooleanWithDefault(this[index], defaultValue);
        }

        /// <summary>
        /// Converts array element into an integer or returns null if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>integer value of element or null if conversion is not supported.</returns>
        /// See <see cref="IntegerConverter.ToNullableInteger(object)"/>
        public int? GetAsNullableInteger(int index)
        {
            return IntegerConverter.ToNullableInteger(this[index]);
        }

        /// <summary>
        /// Converts array element into an integer or returns 0 if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>integer value of element or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsIntegerWithDefault(int, int)"/>
        public int GetAsInteger(int index)
        {
            return GetAsIntegerWithDefault(index, 0);
        }

        /// <summary>
        /// Converts array element into an integer or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>integer value of element or default value if conversion is not supported.</returns>
        /// See <see cref="IntegerConverter.ToIntegerWithDefault(object, int)"/>
        public int GetAsIntegerWithDefault(int index, int defaultValue)
        {
            return IntegerConverter.ToIntegerWithDefault(this[index], defaultValue);
        }

        /// <summary>
        /// Converts array element into a long or returns null if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>long value of element or null if conversion is not supported.</returns>
        /// See <see cref="LongConverter.ToNullableLong(object)"/>
        public long? GetAsNullableLong(int index)
        {
            return LongConverter.ToNullableLong(this[index]);
        }

        /// <summary>
        /// Converts array element into a long or returns 0 if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>long value of element or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsLongWithDefault(int, long)"/>
        public long GetAsLong(int index)
        {
            return GetAsLongWithDefault(index, 0);
        }

        /// <summary>
        /// Converts array element into a long or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>long value of element or default value if conversion is not supported.</returns>
        /// See <see cref="LongConverter.ToLongWithDefault(object, long)"/>
        public long GetAsLongWithDefault(int index, long defaultValue)
        {
            return LongConverter.ToLongWithDefault(this[index], defaultValue);
        }

        /// <summary>
        /// Converts array element into a float or returns null if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>float value of element or null if conversion is not supported.</returns>
        /// See <see cref="FloatConverter.ToNullableFloat(object)"/>
        public float? GetAsNullableFloat(int index)
        {
            return FloatConverter.ToNullableFloat(this[index]);
        }

        /// <summary>
        /// Converts array element into a float or returns 0 if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>float value of element or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsFloatWithDefault(int, float)"/>
        public float GetAsFloat(int index)
        {
            return GetAsFloatWithDefault(index, 0);
        }

        /// <summary>
        /// Converts array element into a float or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>float value of element or default value if conversion is not supported.</returns>
        /// See <see cref="FloatConverter.ToFloatWithDefault(object, float)"/>
        public float GetAsFloatWithDefault(int index, float defaultValue)
        {
            return FloatConverter.ToFloatWithDefault(this[index], defaultValue);
        }

        /// <summary>
        /// Converts array element into a double or returns null if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>double value of element or null if conversion is not supported.</returns>
        /// See <see cref="DoubleConverter.ToNullableDouble(object)"/>
        public double? GetAsNullableDouble(int index)
        {
            return DoubleConverter.ToNullableDouble(this[index]);
        }

        /// <summary>
        /// Converts array element into a double or returns 0 if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>double value of element or 0 if conversion is not supported.</returns>
        /// See <see cref="GetAsDoubleWithDefault(int, double)"/>
        public double GetAsDouble(int index)
        {
            return GetAsDoubleWithDefault(index, 0);
        }

        /// <summary>
        /// Converts array element into a double or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>double value of element or default value if conversion is not supported.</returns>
        /// See <see cref="DoubleConverter.ToDoubleWithDefault(object, double)"/>
        public double GetAsDoubleWithDefault(int index, double defaultValue)
        {
            return DoubleConverter.ToDoubleWithDefault(this[index], defaultValue);
        }

        /// <summary>
        /// Converts array element into a Date or returns null if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>DateTime value of element or null if conversion is not supported.</returns>
        /// See <see cref="DateTimeConverter.ToNullableDateTime(object)"/>
        public DateTime? GetAsNullableDateTime(int index)
        {
            return DateTimeConverter.ToNullableDateTime(this[index]);
        }

        /// <summary>
        /// Converts array element into a Date or returns current date if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>DateTime value of element or current date if conversion is not supported.</returns>
        /// See <see cref="GetAsDateTimeWithDefault(int, DateTime?)"/>
        public DateTime GetAsDateTime(int index)
        {
            return GetAsDateTimeWithDefault(index, new DateTime());
        }

        /// <summary>
        /// Converts array element into a Date or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>DateTime value of element or default value if conversion is not supported.</returns>
        /// See <see cref="DateTimeConverter.ToDateTimeWithDefault(object, DateTime?)"/>
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

        /// <summary>
        /// Converts array element into a value defined by specied typecode. 
        /// If conversion is not possible it returns default value for the specified type..
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="index">an index of element to get.</param>
        /// <returns>value of element defined by the typecode or default value for the specified type. if conversion is not supported.</returns>
        /// See <see cref="TypeConverter.ToType{T}(object)"/>
        public T GetATypes<T>(int index)
        {
            return TypeConverter.ToType<T>(this[index]);
        }

        /// <summary>
        /// Converts array element into a value defined by specied typecode. 
        /// If conversion is not possible it returns null.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="index">an index of element to get.</param>
        /// <returns>value of element defined by the typecode or null if conversion is not supported.</returns>
        /// See <see cref="TypeConverter.ToNullableType{T}(object)"/>
        public T? GetAsNullableType<T>(int index) where T : struct
        {
            return TypeConverter.ToNullableType<T>(this[index]);
        }

        /// <summary>
        /// Converts array element into a value defined by specied typecode. 
        /// If conversion is not possible it returns default value.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="index">an index of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>value of element defined by the typecode or default value if conversion is not supported.</returns>
        /// See <see cref="TypeConverter.ToTypeWithDefault{T}(object, T)"/>
        public T GetAsNullableTypeWithDefault<T>(int index, T defaultValue)
        {
            return TypeConverter.ToTypeWithDefault<T>(this[index], defaultValue);
        }

        /// <summary>
        /// Converts array element into an AnyValue or returns an empty AnyValue if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>AnyValue value of the element or empty AnyValue if conversion is not supported.</returns>
        /// See <see cref="AnyValue"/>, <see cref="AnyValue.AnyValue(object)"/>
        public AnyValue GetAsValue(int index)
        {
            return new AnyValue(this[index]);
        }

        /// <summary>
        /// Converts array element into an AnyValueArray or returns null if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>AnyValueArray value of the element or null if conversion is not supported.</returns>
        public AnyValueArray GetAsNullableArray(int index)
        {
            var value = GetAsObject(index);
            return value != null ? new AnyValueArray(new[] { value }) : null;
        }

        /// <summary>
        /// Converts array element into an AnyValueArray or returns empty AnyValueArray if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>AnyValueArray value of the element or empty AnyValueArray if conversion is not supported.</returns>
        public AnyValueArray GetAsArray(int index)
        {
            return new AnyValueArray(new[] { this[index] });
        }

        /// <summary>
        /// Converts array element into an AnyValueArray or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>AnyValueArray value of the element or default value if conversion is not supported.</returns>
        public AnyValueArray GetAsArrayWithDefault(int index, AnyValueArray defaultValue)
        {
            var result = GetAsNullableArray(index);
            return result ?? defaultValue;
        }

        /// <summary>
        /// Converts array element into an AnyValueMap or returns null if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>AnyValueMap value of the element or null if conversion is not supported.</returns>
        /// See <see cref="AnyValueMap"/>
        public AnyValueMap GetAsNullableMap(int index)
        {
            var value = GetAsObject(index);
            return value != null ? AnyValueMap.FromValue(value) : null;
        }

        /// <summary>
        /// Converts array element into an AnyValueMap or returns empty AnyValueMap if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <returns>AnyValueMap value of the element or empty AnyValueMap if conversion is not supported.</returns>
        /// See <see cref="AnyValueMap"/>
        public AnyValueMap GetAsMap(int index)
        {
            return AnyValueMap.FromValue(GetAsObject(index));
        }

        /// <summary>
        /// Converts array element into an AnyValueMap or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="index">an index of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>nyValueMap value of the element or default value if conversion is not supported</returns>
        /// See <see cref="GetAsNullableMap(int)"/>
        public AnyValueMap GetAsMapWithDefault(int index, AnyValueMap defaultValue)
        {
            var result = GetAsNullableMap(index);
            return result ?? defaultValue;
        }

        /// <summary>
        /// Checks if this array contains a value. The check uses direct comparison between elements and the specified value.
        /// </summary>
        /// <param name="value">a value to be checked</param>
        /// <returns>true if this array contains the value or false otherwise.</returns>
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

        /// <summary>
        /// Checks if this array contains a value. The check before comparison converts
        /// elements and the value to type specified by type code.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="value">a value to be checked</param>
        /// <returns>true if this array contains the value or false otherwise.</returns>
        /// See <see cref="TypeConverter.ToType{T}(object)"/>
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

        /// <summary>
        /// Gets a string representation of the object. The result is a comma-separated
        /// list of string representations of individual elements as "value1,value2,value3"
        /// </summary>
        /// <returns>a string representation of the object.</returns>
        /// See <see cref="StringConverter.ToString(object)"/>
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

        /// <summary>
        /// Creates a binary clone of this object.
        /// </summary>
        /// <returns>a clone of this object.</returns>
        public object Clone()
        {
            return new AnyValueArray(this);
        }

        /// <summary>
        /// Creates a new AnyValueArray from a list of values
        /// </summary>
        /// <param name="values">a list of values to initialize the created AnyValueArray</param>
        /// <returns>a newly created AnyValueArray.</returns>
        public static AnyValueArray FromValues(params object[] values)
        {
            return new AnyValueArray(values);
        }

        /// <summary>
        /// Converts specified value into AnyValueArray.
        /// </summary>
        /// <param name="value">value to be converted</param>
        /// <returns>a newly created AnyValueArray.</returns>
        /// See <see cref="ArrayConverter.ToNullableArray(object)"/>
        public static AnyValueArray FromValue(object value)
        {
            var values = ArrayConverter.ToNullableArray(value);
            return new AnyValueArray(values);
        }

        /// <summary>
        /// Splits specified string into elements using a separator and assigns the elements to a newly created AnyValueArray.
        /// </summary>
        /// <param name="value">a string value to be split and assigned to AnyValueArray</param>
        /// <param name="separator">a separator to split the string</param>
        /// <param name="removeDuplicates">(optional) true to remove duplicated elements</param>
        /// <returns>a newly created AnyValueArray.</returns>
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

        /// <summary>
        /// Splits specified string into elements using a separator and assigns the elements to a newly created AnyValueArray.
        /// </summary>
        /// <param name="value">a string value to be split and assigned to AnyValueArray</param>
        /// <param name="separator">a separator to split the string</param>
        /// <returns>a newly created AnyValueArray.</returns>
        public static AnyValueArray FromString(string value, char separator)
        {
            return FromString(value, separator, false);
        }
    }
}