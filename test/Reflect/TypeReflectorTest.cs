using System;
using Xunit;

namespace PipServices.Commons.Reflect
{
    //[TestClass]
    public class TypeReflectorTest
    {
        [Fact]
        public void TestGetType()
        {
            Type type = TypeReflector.GetType("PipServices.Commons.Convert.TypeCode");
            Assert.NotNull(type);

            type = TypeReflector.GetType("PipServices.Commons.Convert.TypeCode", "PipServices.Commons");
            Assert.NotNull(type);
        }

        [Fact]
        public void TestCreateInstance()
        {
            //object value = TypeReflector.CreateInstance("PipServices.Commons.Reflect.TestClass");
            //Assert.NotNull(value);
        }
    }
}
