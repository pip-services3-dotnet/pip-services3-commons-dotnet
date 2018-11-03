using System;
using Xunit;

namespace PipServices3.Commons.Reflect
{
    //[TestClass]
    public class TypeReflectorTest
    {
        [Fact]
        public void TestGetType()
        {
            Type type = TypeReflector.GetType("PipServices3.Commons.Convert.TypeCode");
            Assert.NotNull(type);

            type = TypeReflector.GetType("PipServices3.Commons.Convert.TypeCode", "PipServices3.Commons");
            Assert.NotNull(type);
        }

        [Fact]
        public void TestCreateInstance()
        {
            //object value = TypeReflector.CreateInstance("PipServices3.Commons.Reflect.TestClass");
            //Assert.NotNull(value);
        }
    }
}
