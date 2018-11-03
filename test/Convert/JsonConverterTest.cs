using PipServices3.Commons.Convert;
using PipServices3.Commons.Data;
using System;
using System.Collections.Generic;
using Xunit;

namespace PipServices3.Commons.Test.Convert
{
    //[TestClass]
    public sealed class JsonConverterTest
    {
        [Fact]
        public void TestToJson()
        {
            Assert.Null(JsonConverter.ToJson(null));
            Assert.Equal("123", JsonConverter.ToJson(123));
            Assert.Equal("\"ABC\"", JsonConverter.ToJson("ABC"));

            var filter = FilterParams.FromTuples("Key1", 123, "Key2", "ABC");
            var jsonFilter = JsonConverter.ToJson(filter);
            Assert.Equal("{\"Key1\":\"123\",\"Key2\":\"ABC\"}", jsonFilter);

            var array = AnyValueArray.FromValues(123, "ABC");
            var jsonArray = JsonConverter.ToJson(array);
            Assert.Equal("[123,\"ABC\"]", jsonArray);

            var date = DateTimeConverter.ToDateTime("1975-04-08T00:00:00.000Z");
            var jsonDate = JsonConverter.ToJson(date);
            Assert.Equal("\"1975-04-08T00:00:00Z\"", jsonDate);
        }

        [Fact]
        public void TestFromJson()
        {
            Assert.Null(JsonConverter.ToJson(null));

            Assert.Equal(123, (int) JsonConverter.FromJson<int>("123"));

            Assert.Equal("ABC", JsonConverter.FromJson<string>("\"ABC\""));

            var filter = JsonConverter.FromJson<FilterParams>("{\"Key2\":\"ABC\",\"Key1\":\"123\"}");
            Assert.Equal(2, filter.Count);

            var array = JsonConverter.FromJson<AnyValueArray>("[123,\"ABC\"]");
            Assert.Equal(2, array.Count);

            var date = DateTimeConverter.ToDateTime("1975-04-08T00:00:00.000Z");
            var jsonDate = JsonConverter.FromJson<DateTimeOffset>("\"1975-04-08T00:00Z\"");

            Assert.Equal(date.Year, jsonDate.Year);
            Assert.Equal(date.Month, jsonDate.Month);
            Assert.Equal(date.Day, jsonDate.Day);
        }


        [Fact]
        public void TestJsonToMap()
        {
            // Handling simple objects
            var value = "{ \"value1\":123, \"value2\":234 }";
            var result = JsonConverter.ToNullableMap(value);
            Assert.Equal(123, (int)(long)result["value1"]);
            Assert.Equal(234, (int)(long)result["value2"]);

            // Recursive conversion
            value = "{ \"value1\":123, \"value2\": { \"value1\": 111, \"value2\": 222 } }";
            result = JsonConverter.ToNullableMap(value);
            Assert.NotNull(result);
            Assert.Equal(123, (int)(long)result["value1"]);

            Assert.NotNull(result["value2"]);
            Assert.True(result["value2"] is IDictionary<string, object>);

            // Handling arrays
            value = "{ \"value1\": [{ \"value1\": 111, \"value2\": 222 }] }";
            result = JsonConverter.ToNullableMap(value);
            Assert.NotNull(result);

            Assert.True(result["value1"] is IList<object>);
            var resultElements = ((IList<object>) result["value1"]);

            var resultElement0 = (IDictionary<string, object>) resultElements[0];

            Assert.NotNull(resultElement0);
            Assert.Equal(111, (int)(long)resultElement0["value1"]);
            Assert.Equal(222, (int)(long)resultElement0["value2"]);
        }
    }
}
