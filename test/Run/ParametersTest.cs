using PipServices3.Commons.Run;
using PipServices3.Commons.Config;
using Xunit;

namespace PipServices3.Commons.Test.Run
{
    //[TestClass]
    public sealed class ParametersTest
    {
        [Fact]
        public void TestDefaults()
        {
            var result = Parameters.FromTuples(
                "value1", 123,
                "value2", 234
            );
            var defaults = Parameters.FromTuples(
                "value2", 432,
                "value3", 345
            );
            result = result.SetDefaults(defaults, false);
            Assert.Equal(3, result.Count);
            Assert.Equal(123, result["value1"]);
            Assert.Equal(234, result["value2"]);
            Assert.Equal(345, result["value3"]);
        }

        [Fact]
        public void TestOverrideRecursive()
        {
            var result = Parameters.FromJson(
                "{ \"value1\": 123, \"value2\": { \"value21\": 111, \"value22\": 222 } }"
            );
            var defaults = Parameters.FromJson(
                "{ \"value2\": { \"value22\": 777, \"value23\": 333 }, \"value3\": 345 }"
            );
            result = result.SetDefaults(defaults, true);

            Assert.Equal(3, result.Count);
            Assert.Equal(123, (int)(long)result.Get("value1"));
            Assert.Equal(345, (int)(long)result.Get("value3"));

            var deepResult = result.GetAsMap("value2");
            Assert.Equal(3, deepResult.Count);
            Assert.Equal(111, (int)(long)deepResult.Get("value21"));
            Assert.Equal(222, (int)(long)deepResult.Get("value22"));
            Assert.Equal(333, (int)(long)deepResult.Get("value23"));
        }

        [Fact]
        public void TestOverrideWithNulls()
        {
            var result = Parameters.FromJson(
                "{ \"value1\": 123, \"value2\": 234 }"
            );
            result = result.Override(null, true);

            Assert.Equal(2, result.Count);
            Assert.Equal(123, (int)(long)result.Get("value1"));
            Assert.Equal(234, (int)(long)result.Get("value2"));
        }

        [Fact]
        public void TestAssignTo()
        {
            var value = new TestClass(null, null);
            var newValues = Parameters.FromJson(
                "{ \"value1\": 123, \"value2\": \"ABC\", \"value3\": 456 }"
            );

            newValues.AssignTo(value);
            Assert.NotNull(value.Value1);
            Assert.Equal(123, (int)(long)value.Value1);
            Assert.NotNull(value.Value2);
            Assert.Equal("ABC", value.Value2);
        }

        [Fact]
        public void TestGet()
        {
            var config = Parameters.FromJson(
                "{ \"value1\": 123, \"value2\": { \"value21\": 111, \"value22\": 222 } }"
            );

            var value = config.Get("");
            Assert.Null(value);

            value = config.Get("value1");
            Assert.NotNull(value);
            Assert.Equal(123, (int)(long)value);

            value = config.Get("value2");
            Assert.NotNull(value);

            var boolVal = config.ContainsKey("value3");
            Assert.False(boolVal);

            value = config.Get("value2.value21");
            Assert.NotNull(value);
            Assert.Equal(111, (int)(long)value);

            boolVal = config.ContainsKey("value2.value31");
            Assert.False(boolVal);

            boolVal = config.ContainsKey("value2.value21.value211");
            Assert.False(boolVal);

            boolVal = config.ContainsKey("valueA.valueB.valueC");
            Assert.False(boolVal);
        }

        [Fact]
        public void TestContains()
        {
            var config = Parameters.FromJson(
                "{ \"value1\": 123, \"value2\": { \"value21\": 111, \"value22\": 222 } }"
            );

            var has = config.ContainsKey("");
            Assert.False(has);

            has = config.ContainsKey("value1");
            Assert.True(has);

            has = config.ContainsKey("value2");
            Assert.True(has);

            has = config.ContainsKey("value3");
            Assert.False(has);

            has = config.ContainsKey("value2.value21");
            Assert.True(has);

            has = config.ContainsKey("value2.value31");
            Assert.False(has);

            has = config.ContainsKey("value2.value21.value211");
            Assert.False(has);

            has = config.ContainsKey("valueA.valueB.valueC");
            Assert.False(has);
        }

        [Fact]
        public void TestSet()
        {
            var config = new Parameters();

            config.Set(null, 123);
            Assert.Equal(0, config.Count);

            config.Set("field1", 123);
            Assert.Equal(1, config.Count);
            Assert.Equal(123, (int)config.Get("field1"));

            config.Set("field2", "ABC");
            Assert.Equal(2, config.Count);
            Assert.Equal("ABC", config.Get("field2"));

            config.Set("field2.field1", 123);
            Assert.Equal("ABC", config.Get("field2"));

            config.Set("field3.field31", 456);
            Assert.Equal(3, config.Count);
            var subConfig = config.GetAsMap("field3");
            Assert.NotNull(subConfig);
            Assert.Equal(456, (int)subConfig.Get("field31"));

            config.Set("field3.field32", "XYZ");
            Assert.Equal("XYZ", config.Get("field3.field32"));
        }

        [Fact]
        public void TestFromConfig()
        {
            var config = ConfigParams.FromTuples(
                "field1.field11", 123,
                "field2", "ABC",
                "field1.field12", "XYZ"
            );

            var parameters = Parameters.FromConfig(config);
            Assert.Equal(2, parameters.Count);
            Assert.Equal("ABC", parameters.Get("field2"));

            var value = parameters.GetAsMap("field1");
            Assert.Equal(2, value.Count);
            Assert.Equal("123", value.Get("field11"));
            Assert.Equal("XYZ", value.Get("field12"));
        }
    }
}
