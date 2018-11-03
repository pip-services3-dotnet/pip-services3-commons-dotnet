using System.Collections.Generic;
using Xunit;
using PipServices3.Commons.Validate;
using PipServices3.Commons.Convert;

namespace PipServices3.Commons.Test.Validate
{
    //[TestClass]
    public class SchemaTest
    {
        [Fact]
        public void TestEmptySchema()
        {
            var schema = new ObjectSchema();
            var results = schema.Validate(null);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void TestRequired()
        {
            var schema = new Schema().MakeRequired();
            var results = schema.Validate(null);
            Assert.Equal(1, results.Count);
        }

        [Fact]
        public void TestUnexpected()
        {
            var schema = new ObjectSchema();
            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Equal(8, results.Count);
        }

        [Fact]
        public void TestOptionalProperties()
        {
            var schema = new ObjectSchema()
                .WithOptionalProperty("intField", null)
                .WithOptionalProperty("StringProperty", null)
                .WithOptionalProperty("NullProperty", null)
                .WithOptionalProperty("IntArrayProperty", null)
                .WithOptionalProperty("StringListProperty", null)
                .WithOptionalProperty("MapProperty", null)
                .WithOptionalProperty("SubObjectProperty", null)
                .WithOptionalProperty("SubArrayProperty", null);

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void TestRequiredProperties()
        {
            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", null)
                .WithRequiredProperty("StringProperty", null)
                .WithRequiredProperty("NullProperty", null)
                .WithRequiredProperty("IntArrayProperty", null)
                .WithRequiredProperty("StringListProperty", null)
                .WithRequiredProperty("MapProperty", null)
                .WithRequiredProperty("SubObjectProperty", null)
                .WithRequiredProperty("SubArrayProperty", null);

            var obj = new TestObject {SubArrayProperty = null};

            var results = schema.Validate(obj);
            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void TestObjectTypes()
        {
            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", typeof(int))
                .WithRequiredProperty("StringProperty", typeof(string))
                .WithOptionalProperty("NullProperty", typeof(object))
                .WithRequiredProperty("IntArrayProperty", typeof(int[]))
                .WithRequiredProperty("StringListProperty", typeof(List<string>))
                .WithRequiredProperty("MapProperty", typeof(Dictionary<string, int>))
                .WithRequiredProperty("SubObjectProperty", typeof(TestSubObject))
                .WithRequiredProperty("SubArrayProperty", typeof(TestSubObject[]));

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void TestStringTypes()
        {
            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", "Int32")
                .WithRequiredProperty("StringProperty", "String")
                .WithOptionalProperty("NullProperty", "Object")
                .WithRequiredProperty("IntArrayProperty", "Int32[]")
                .WithRequiredProperty("StringListProperty", "List`1")
                .WithRequiredProperty("MapProperty", "Dictionary`2")
                .WithRequiredProperty("SubObjectProperty", "TestSubObject")
                .WithRequiredProperty("SubArrayProperty", "TestSubObject[]");

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void TestSubSchema()
        {
            var subSchema = new ObjectSchema()
                .WithRequiredProperty("Id", "String")
                .WithRequiredProperty("FLOATFIELD", "Single")
                .WithOptionalProperty("nullproperty", "Object");

            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", "Int32")
                .WithRequiredProperty("StringProperty", "String")
                .WithOptionalProperty("NullProperty", "Object")
                .WithRequiredProperty("IntArrayProperty", "Int32[]")
                .WithRequiredProperty("StringListProperty", "List`1")
                .WithRequiredProperty("MapProperty", "Dictionary`2")
                .WithRequiredProperty("SubObjectProperty", subSchema)
                .WithRequiredProperty("SubArrayProperty", "TestSubObject[]");

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void TestArrayAndMapSchema()
        {
            var subSchema = new ObjectSchema()
                .WithRequiredProperty("Id", "String")
                .WithRequiredProperty("FLOATFIELD", "Single")
                .WithOptionalProperty("nullproperty", "Object");

            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", "Int32")
                .WithRequiredProperty("StringProperty", "String")
                .WithOptionalProperty("NullProperty", "Object")
                .WithRequiredProperty("IntArrayProperty", new ArraySchema("Int32"))
                .WithRequiredProperty("StringListProperty", new ArraySchema("String"))
                .WithRequiredProperty("MapProperty", new MapSchema("String", "Int32"))
                .WithRequiredProperty("SubObjectProperty", subSchema)
                .WithRequiredProperty("SubArrayProperty", new ArraySchema(subSchema));

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void TestJsonSchema()
        {
            var subSchema = new ObjectSchema()
                .WithRequiredProperty("Id", "string")
                .WithRequiredProperty("FLOATFIELD", "float")
                .WithOptionalProperty("nullproperty", "object");

            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", "int")
                .WithRequiredProperty("StringProperty", "string")
                .WithOptionalProperty("NullProperty", "object")
                .WithRequiredProperty("IntArrayProperty", new ArraySchema("int"))
                .WithRequiredProperty("StringListProperty", new ArraySchema("string"))
                .WithRequiredProperty("MapProperty", new MapSchema("string", "int"))
                .WithRequiredProperty("SubObjectProperty", subSchema)
                .WithRequiredProperty("SubArrayProperty", new ArraySchema(subSchema));

            var obj = new TestObject();
            var json = JsonConverter.ToJson(obj);
            var jsonObj = JsonConverter.FromJson(json);

            var results = schema.Validate(jsonObj);
            Assert.Equal(0, results.Count);
        }
    }
}
