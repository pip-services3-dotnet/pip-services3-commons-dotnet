using Xunit;
using PipServices3.Commons.Reflect;
using PipServices3.Commons.Convert;

namespace PipServices3.Commons.Test.Reflect
{
    //[TestClass]
    public sealed class RecursiveObjectReaderTest
    {
        [Fact]
        public void TestHasProperty()
        {
            var obj = JsonConverter.ToMap(
                "{ \"value1\": 123, \"value2\": { \"value21\": 111, \"value22\": 222 }, \"value3\": [ 444, { \"value311\": 555 } ] }"
            );

            var has = RecursiveObjectReader.HasProperty(obj, "");
            Assert.False(has);

            has = RecursiveObjectReader.HasProperty(obj, "value1");
            Assert.True(has);

            has = RecursiveObjectReader.HasProperty(obj, "value2");
            Assert.True(has);

            has = RecursiveObjectReader.HasProperty(obj, "value2.value21");
            Assert.True(has);

            has = RecursiveObjectReader.HasProperty(obj, "value2.value31");
            Assert.False(has);

            has = RecursiveObjectReader.HasProperty(obj, "value2.value21.value211");
            Assert.False(has);

            has = RecursiveObjectReader.HasProperty(obj, "valueA.valueB.valueC");
            Assert.False(has);

            has = RecursiveObjectReader.HasProperty(obj, "value3");
            Assert.True(has);

            has = RecursiveObjectReader.HasProperty(obj, "value3.0");
            Assert.True(has);

            has = RecursiveObjectReader.HasProperty(obj, "value3.0.value311");
            Assert.False(has);

            has = RecursiveObjectReader.HasProperty(obj, "value3.1");
            Assert.True(has);

            has = RecursiveObjectReader.HasProperty(obj, "value3.1.value311");
            Assert.True(has);

            has = RecursiveObjectReader.HasProperty(obj, "value3.2");
            Assert.False(has);
        }

        [Fact]
        public void TestGetProperty()
        {
            var obj = JsonConverter.ToMap(
                "{ \"value1\": 123, \"value2\": { \"value21\": 111, \"value22\": 222 }, \"value3\": [ 444, { \"value311\": 555 } ] }"
            );

            var value = RecursiveObjectReader.GetProperty(obj, "");
            Assert.Null(value);

            value = (int)(long)RecursiveObjectReader.GetProperty(obj, "value1");
            Assert.Equal(123, value);

            value = RecursiveObjectReader.GetProperty(obj, "value2");
            Assert.NotNull(value);

            value = (int)(long)RecursiveObjectReader.GetProperty(obj, "value2.value21");
            Assert.Equal(111, value);

            value = RecursiveObjectReader.GetProperty(obj, "value2.value31");
            Assert.Null(value);

            value = RecursiveObjectReader.GetProperty(obj, "value2.value21.value211");
            Assert.Null(value);

            value = RecursiveObjectReader.GetProperty(obj, "valueA.valueB.valueC");
            Assert.Null(value);

            value = RecursiveObjectReader.GetProperty(obj, "value3");
            Assert.NotNull(value);

            value = (int)(long)RecursiveObjectReader.GetProperty(obj, "value3.0");
            Assert.Equal(444, value);

            value = RecursiveObjectReader.GetProperty(obj, "value3.0.value311");
            Assert.Null(value);

            value = RecursiveObjectReader.GetProperty(obj, "value3.1");
            Assert.NotNull(value);

            value = (int)(long)RecursiveObjectReader.GetProperty(obj, "value3.1.value311");
            Assert.Equal(555, value);

            value = RecursiveObjectReader.GetProperty(obj, "value3.2");
            Assert.Null(value);
        }

        [Fact]
        public void TestGetPropertyNames()
        {
            var obj = JsonConverter.ToMap(
                "{ \"value1\": 123, \"value2\": { \"value21\": 111, \"value22\": 222 }, \"value3\": [ 444, { \"value311\": 555 } ] }"
            );

            var names = RecursiveObjectReader.GetPropertyNames(obj);
            Assert.Equal(5, names.Count);
            Assert.True(names.Contains("value1"));
            Assert.True(names.Contains("value2.value21"));
            Assert.True(names.Contains("value2.value22"));
            Assert.True(names.Contains("value3.0"));
            Assert.True(names.Contains("value3.1.value311"));
        }

        [Fact]
        public void TestGetProperties()
        {
            var obj = JsonConverter.ToMap(
                "{ \"value1\": 123, \"value2\": { \"value21\": 111, \"value22\": 222 }, \"value3\": [ 444, { \"value311\": 555 } ] }"
            );

            var values = RecursiveObjectReader.GetProperties(obj);
            Assert.Equal(5, values.Count);
            Assert.Equal(123, (int)(long)values["value1"]);
            Assert.Equal(111, (int)(long)values["value2.value21"]);
            Assert.Equal(222, (int)(long)values["value2.value22"]);
            Assert.Equal(444, (int)(long)values["value3.0"]);
            Assert.Equal(555, (int)(long)values["value3.1.value311"]);
        }
    }
}
