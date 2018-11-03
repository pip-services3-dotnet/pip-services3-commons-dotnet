using PipServices3.Commons.Data;
using System;
using System.Collections.Generic;
using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class TypeConverterTest
    {
        [Fact]
        public void TestToTypeCode()
        {
            Assert.Equal(TypeCode.String, TypeConverter.ToTypeCode(typeof(string)));
            Assert.Equal(TypeCode.Integer, TypeConverter.ToTypeCode(typeof(int)));
            Assert.Equal(TypeCode.Long, TypeConverter.ToTypeCode(typeof(long)));
            Assert.Equal(TypeCode.Float, TypeConverter.ToTypeCode(typeof(float)));
            Assert.Equal(TypeCode.Double, TypeConverter.ToTypeCode(typeof(double)));
            Assert.Equal(TypeCode.DateTime, TypeConverter.ToTypeCode(typeof(DateTime)));
            Assert.Equal(TypeCode.Enum, TypeConverter.ToTypeCode(typeof(LogLevel)));
            Assert.Equal(TypeCode.Array, TypeConverter.ToTypeCode(typeof(List<object>)));
            Assert.Equal(TypeCode.Map, TypeConverter.ToTypeCode(typeof(Dictionary<string, object>)));
            Assert.Equal(TypeCode.Object, TypeConverter.ToTypeCode(typeof(object)));
            Assert.Equal(TypeCode.Unknown, TypeConverter.ToTypeCode(null));

            Assert.Equal(TypeCode.String, TypeConverter.ToTypeCode("123"));
            Assert.Equal(TypeCode.Integer, TypeConverter.ToTypeCode(123));
            Assert.Equal(TypeCode.Long, TypeConverter.ToTypeCode(123L));
            Assert.Equal(TypeCode.Float, TypeConverter.ToTypeCode(123.456f));
            Assert.Equal(TypeCode.Double, TypeConverter.ToTypeCode(123.456));
            Assert.Equal(TypeCode.DateTime, TypeConverter.ToTypeCode(DateTime.Now));
            Assert.Equal(TypeCode.Enum, TypeConverter.ToTypeCode(LogLevel.None));
            Assert.Equal(TypeCode.Array, TypeConverter.ToTypeCode(new List<int>()));
            Assert.Equal(TypeCode.Array, TypeConverter.ToTypeCode(new int[0]));
            Assert.Equal(TypeCode.Map, TypeConverter.ToTypeCode(new Dictionary<string, string>()));
            Assert.Equal(TypeCode.Object, TypeConverter.ToTypeCode(new object()));
	    }

        [Fact]
        public void TestToNullableType()
        {
            Assert.Equal("123", TypeConverter.ToType<string>(123));
            Assert.Equal(123, TypeConverter.ToNullableType<int>("123"));
            Assert.Equal(123L, TypeConverter.ToNullableType<long>(123.456));
            Assert.True(123 - TypeConverter.ToNullableType<float>(123) < 0.001);
            Assert.True(123 - TypeConverter.ToNullableType<double>(123) < 0.001);
            Assert.Equal(DateTimeConverter.ToDateTime("1975-04-08T17:30:00.00Z"), 
                TypeConverter.ToNullableType<DateTime>("1975-04-08T17:30:00.00Z"));
            //Assert.Equal(1, TypeConverter.ToNullableType<List<object>>(123).Count);
            //Assert.Equal(1, TypeConverter.ToNullableType<Dictionary<string, object>>(StringValueMap.FromString("abc=123")).Count);
	    }

        [Fact]
        public void TestToType()
        {
            Assert.Equal("123", TypeConverter.ToType<string>(123));
            Assert.Equal(123, TypeConverter.ToType<int>("123"));
            Assert.Equal(123L, TypeConverter.ToType<long>(123.456));
            Assert.True(123 - TypeConverter.ToType<float>(123) < 0.001);
            Assert.True(123 - TypeConverter.ToType<double>(123) < 0.001);
            Assert.Equal(DateTimeConverter.ToDateTime("1975-04-08T17:30:00.00Z"), 
                TypeConverter.ToType<DateTime>("1975-04-08T17:30:00.00Z"));
            //Assert.Equal(1, TypeConverter.ToType<List<object>>(123).Count);
            //Assert.Equal(1, TypeConverter.ToType<Dictionary<string, object>>(StringValueMap.FromString("abc=123")).Count);
	    }

        [Fact]
        public void TestToTypeWithDefault()
        {
            Assert.Equal("123", TypeConverter.ToTypeWithDefault<string>(null, "123"));
            Assert.Equal(123, TypeConverter.ToTypeWithDefault<int>(null, 123));
            Assert.Equal(123L, TypeConverter.ToTypeWithDefault<long>(null, 123L));
            Assert.True(123 - TypeConverter.ToTypeWithDefault<float>(null, (float)123) < 0.001);
            Assert.True(123 - TypeConverter.ToTypeWithDefault<double>(null, 123.0) < 0.001);
            Assert.Equal(DateTimeConverter.ToDateTime("1975-04-08T17:30:00.00Z"), 
                TypeConverter.ToTypeWithDefault<DateTime>("1975-04-08T17:30:00.00Z", default(DateTime)));
            //Assert.Equal(1, TypeConverter.ToTypeWithDefault<List<object>>(123, null).Count);
            //Assert.Equal(1, TypeConverter.ToTypeWithDefault<Dictionary<string, object>>(StringValueMap.FromString("abc=123"), null).Count);
	    }

    }
}
