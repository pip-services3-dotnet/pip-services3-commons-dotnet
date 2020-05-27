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
            Assert.Empty(results);
        }

        [Fact]
        public void TestRequired()
        {
            var schema = new Schema().MakeRequired();
            var results = schema.Validate(null);
            Assert.Single(results);
        }

        [Fact]
        public void TestUnexpected()
        {
            var schema = new ObjectSchema();
            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Equal(13, results.Count);
        }

        [Fact]
        public void TestOptionalProperties()
        {
            var schema = new ObjectSchema()
                .WithOptionalProperty("intField", null)
                .WithOptionalProperty("longField", null)
                .WithRequiredProperty("floatField", null)
                .WithRequiredProperty("doubleField", null)
                .WithOptionalProperty("StringProperty", null)
                .WithOptionalProperty("NullProperty", null)
                .WithOptionalProperty("IntArrayProperty", null)
                .WithOptionalProperty("StringListProperty", null)
                .WithOptionalProperty("MapProperty", null)
                .WithOptionalProperty("SubObjectProperty", null)
                .WithOptionalProperty("SubArrayProperty", null)
                .WithOptionalProperty("EnumIntProperty", null)
                .WithOptionalProperty("EnumStringProperty", null);

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Empty(results);
        }

        [Fact]
        public void TestRequiredProperties()
        {
            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", null)
                .WithRequiredProperty("longField", null)
                .WithRequiredProperty("floatField", null)
                .WithRequiredProperty("doubleField", null)
                .WithRequiredProperty("StringProperty", null)
                .WithRequiredProperty("NullProperty", null)
                .WithRequiredProperty("IntArrayProperty", null)
                .WithRequiredProperty("StringListProperty", null)
                .WithRequiredProperty("MapProperty", null)
                .WithRequiredProperty("SubObjectProperty", null)
                .WithRequiredProperty("SubArrayProperty", null)
                .WithRequiredProperty("EnumIntProperty", null)
                .WithRequiredProperty("EnumStringProperty", null);

            var obj = new TestObject {SubArrayProperty = null};

            var results = schema.Validate(obj);
            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void TestObjectTypes()
        {
            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", typeof(int))
                .WithRequiredProperty("longField", typeof(long))
                .WithRequiredProperty("floatField", typeof(float))
                .WithRequiredProperty("doubleField", typeof(double))
                .WithRequiredProperty("StringProperty", typeof(string))
                .WithOptionalProperty("NullProperty", typeof(object))
                .WithRequiredProperty("IntArrayProperty", typeof(int[]))
                .WithRequiredProperty("StringListProperty", typeof(List<string>))
                .WithRequiredProperty("MapProperty", typeof(Dictionary<string, int>))
                .WithRequiredProperty("SubObjectProperty", typeof(TestSubObject))
                .WithRequiredProperty("SubArrayProperty", typeof(TestSubObject[]))
                .WithRequiredProperty("EnumIntProperty", typeof(TestEnumInt))
                .WithRequiredProperty("EnumStringProperty", typeof(TestEnumString));

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Empty(results);
        }

        [Fact]
        public void TestStringTypes()
        {
            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", "Int32")
                .WithRequiredProperty("longField", "Int64")
                .WithRequiredProperty("floatField", "Float")
                .WithRequiredProperty("doubleField", "Double")
                .WithRequiredProperty("StringProperty", "String")
                .WithOptionalProperty("NullProperty", "Object")
                .WithRequiredProperty("IntArrayProperty", "Int32[]")
                .WithRequiredProperty("StringListProperty", "List`1")
                .WithRequiredProperty("MapProperty", "Dictionary`2")
                .WithRequiredProperty("SubObjectProperty", "TestSubObject")
                .WithRequiredProperty("SubArrayProperty", "TestSubObject[]")
                .WithRequiredProperty("EnumIntProperty", "TestEnumInt")
                .WithRequiredProperty("EnumStringProperty", "TestEnumString");

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Empty(results);
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
                .WithRequiredProperty("longField", "Int64")
                .WithRequiredProperty("floatField", "Float")
                .WithRequiredProperty("doubleField", "Double")
                .WithRequiredProperty("StringProperty", "String")
                .WithOptionalProperty("NullProperty", "Object")
                .WithRequiredProperty("IntArrayProperty", "Int32[]")
                .WithRequiredProperty("StringListProperty", "List`1")
                .WithRequiredProperty("MapProperty", "Dictionary`2")
                .WithRequiredProperty("SubObjectProperty", subSchema)
                .WithRequiredProperty("SubArrayProperty", "TestSubObject[]")
                .WithRequiredProperty("EnumIntProperty", "TestEnumInt")
                .WithRequiredProperty("EnumStringProperty", "TestEnumString");

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Empty(results);
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
                .WithRequiredProperty("longField", "Int64")
                .WithRequiredProperty("floatField", "Float")
                .WithRequiredProperty("doubleField", "Double")
                .WithRequiredProperty("StringProperty", "String")
                .WithOptionalProperty("NullProperty", "Object")
                .WithRequiredProperty("IntArrayProperty", new ArraySchema("Int32"))
                .WithRequiredProperty("StringListProperty", new ArraySchema("String"))
                .WithRequiredProperty("MapProperty", new MapSchema("String", "Int32"))
                .WithRequiredProperty("SubObjectProperty", subSchema)
                .WithRequiredProperty("SubArrayProperty", new ArraySchema(subSchema))
                .WithRequiredProperty("EnumIntProperty", "TestEnumInt")
                .WithRequiredProperty("EnumStringProperty", "TestEnumString");

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Empty(results);
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
                .WithRequiredProperty("longField", "long")
                .WithRequiredProperty("floatField", "float")
                .WithRequiredProperty("doubleField", "double")
                .WithRequiredProperty("StringProperty", "string")
                .WithOptionalProperty("NullProperty", "object")
                .WithRequiredProperty("IntArrayProperty", new ArraySchema("int"))
                .WithRequiredProperty("StringListProperty", new ArraySchema("string"))
                .WithRequiredProperty("MapProperty", new MapSchema("string", "int"))
                .WithRequiredProperty("SubObjectProperty", subSchema)
                .WithRequiredProperty("SubArrayProperty", new ArraySchema(subSchema))
                .WithRequiredProperty("EnumIntProperty", typeof(TestEnumInt))
                .WithRequiredProperty("EnumStringProperty", typeof(TestEnumString));

            var obj = new TestObject();
            var json = JsonConverter.ToJson(obj);
            var jsonObj = JsonConverter.FromJson(json);
            
            var results = schema.Validate(jsonObj);
            Assert.Empty(results);
        }

        [Fact]
        public void TestDynamicTypes()
        {
            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", typeof(long))
                .WithRequiredProperty("longField", typeof(int?))
                .WithRequiredProperty("floatField", typeof(double))
                .WithRequiredProperty("doubleField", typeof(float?))
                .WithRequiredProperty("StringProperty", typeof(string))
                .WithOptionalProperty("NullProperty", typeof(object))
                .WithRequiredProperty("IntArrayProperty", typeof(int[]))
                .WithRequiredProperty("StringListProperty", typeof(List<string>))
                .WithRequiredProperty("MapProperty", typeof(Dictionary<string, int>))
                .WithRequiredProperty("SubObjectProperty", typeof(TestSubObject))
                .WithRequiredProperty("SubArrayProperty", typeof(TestSubObject[]))
                .WithRequiredProperty("EnumIntProperty", typeof(TestEnumInt))
                .WithRequiredProperty("EnumStringProperty", typeof(TestEnumString));

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Empty(results);
        }

        [Fact]
        public void TestTypeCodeTypes()
        {
            var subSchema = new ObjectSchema()
               .WithRequiredProperty("Id", TypeCode.String)
               .WithRequiredProperty("FLOATFIELD", TypeCode.Float)
               .WithOptionalProperty("nullproperty", TypeCode.Object);

            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", TypeCode.Integer)
                .WithRequiredProperty("longField", TypeCode.Long)
                .WithRequiredProperty("floatField", TypeCode.Float)
                .WithRequiredProperty("doubleField", TypeCode.Double)
                .WithRequiredProperty("StringProperty", TypeCode.String)
                .WithOptionalProperty("NullProperty", TypeCode.Object)
                .WithRequiredProperty("IntArrayProperty", new ArraySchema(TypeCode.Integer))
                .WithRequiredProperty("StringListProperty", new ArraySchema(TypeCode.String))
                .WithRequiredProperty("MapProperty", new MapSchema(TypeCode.String, TypeCode.Integer))
                .WithRequiredProperty("SubObjectProperty", subSchema)
                .WithRequiredProperty("SubArrayProperty", new ArraySchema(subSchema))
                .WithRequiredProperty("EnumIntProperty", typeof(TestEnumInt))
                .WithRequiredProperty("EnumStringProperty", typeof(TestEnumString));

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Empty(results);
        }

        [Fact]
        public void TestDynamicTypeCodeTypes()
        {
            var subSchema = new ObjectSchema()
               .WithRequiredProperty("Id", TypeCode.String)
               .WithRequiredProperty("FLOATFIELD", TypeCode.Double)
               .WithOptionalProperty("nullproperty", TypeCode.Object);

            var schema = new ObjectSchema()
                .WithRequiredProperty("intField", TypeCode.Integer)
                .WithRequiredProperty("longField", TypeCode.Integer)
                .WithRequiredProperty("floatField", TypeCode.Double)
                .WithRequiredProperty("doubleField", TypeCode.Float)
                .WithRequiredProperty("StringProperty", TypeCode.String)
                .WithOptionalProperty("NullProperty", TypeCode.Object)
                .WithRequiredProperty("IntArrayProperty", new ArraySchema(TypeCode.Integer))
                .WithRequiredProperty("StringListProperty", new ArraySchema(TypeCode.String))
                .WithRequiredProperty("MapProperty", new MapSchema(TypeCode.String, TypeCode.Integer))
                .WithRequiredProperty("SubObjectProperty", subSchema)
                .WithRequiredProperty("SubArrayProperty", new ArraySchema(subSchema))
                .WithRequiredProperty("EnumIntProperty", typeof(TestEnumInt))
                .WithRequiredProperty("EnumStringProperty", typeof(TestEnumString));

            var obj = new TestObject();
            var results = schema.Validate(obj);
            Assert.Empty(results);
        }
    }
}
