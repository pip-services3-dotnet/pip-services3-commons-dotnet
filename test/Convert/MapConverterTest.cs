using System;
using System.Collections.Generic;
using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class MapConverterTest
    {
        [Fact]
        public void TestToNullableMap()
        {
            Assert.Null(MapConverter.ToNullableMap(null));
            Assert.Null(MapConverter.ToNullableMap(5));

            List<object> array = new List<object>();
            array.Add(1);
            array.Add(2);
            Assert.Equal(MapConverter.ToNullableMap(array).Count, 2);
            Assert.True(MapConverter.ToNullableMap(array).ContainsKey("0"));
            Assert.True(MapConverter.ToNullableMap(array).ContainsKey("1"));
            Assert.Equal(MapConverter.ToNullableMap(array)["0"], 1);
            Assert.Equal(MapConverter.ToNullableMap(array)["1"], 2);
            Assert.True(MapConverter.ToNullableMap(array).Values.Contains(1));
            Assert.True(MapConverter.ToNullableMap(array).Values.Contains(2));

            String[] values = { "ab", "cd" };
            Assert.Equal(MapConverter.ToNullableMap(values).Count, 2);
            Assert.True(MapConverter.ToNullableMap(values).ContainsKey("0"));
            Assert.True(MapConverter.ToNullableMap(values).ContainsKey("1"));
            Assert.Equal(MapConverter.ToNullableMap(values)["0"], "ab");
            Assert.Equal(MapConverter.ToNullableMap(values)["1"], "cd");
            Assert.True(MapConverter.ToNullableMap(values).Values.Contains("ab"));
            Assert.True(MapConverter.ToNullableMap(values).Values.Contains("cd"));

            Dictionary<int, object> map = new Dictionary<int, object>();
            map.Add(8, "title 8");
            map.Add(11, "title 11");
            Assert.Equal(MapConverter.ToNullableMap(map).Count, 2);
            Assert.True(MapConverter.ToNullableMap(map).ContainsKey("8"));
            Assert.True(MapConverter.ToNullableMap(map).ContainsKey("11"));
            Assert.Equal(MapConverter.ToNullableMap(map)["8"], "title 8");
            Assert.Equal(MapConverter.ToNullableMap(map)["11"], "title 11");
            Assert.True(MapConverter.ToNullableMap(map).Values.Contains("title 8"));
            Assert.True(MapConverter.ToNullableMap(map).Values.Contains("title 11"));
        }

        [Fact]
        public void TestToMap()
        {
            Assert.Equal(MapConverter.ToMap(null).Count, 0);
            Assert.Equal(MapConverter.ToMap(5).Count, 0);
        }

        [Fact]
        public void TestToMapWithDefault()
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add("name1", "title 1");
            map.Add("name2", "title 2");
            Assert.Equal(MapConverter.ToMapWithDefault(null, map).Count, 2);
            Assert.Equal(MapConverter.ToMapWithDefault(null, map)["name1"], "title 1");
            Assert.Equal(MapConverter.ToMapWithDefault(null, map)["name2"], "title 2");

            Assert.Equal(MapConverter.ToMapWithDefault(5, map).Count, 2);
            Assert.Equal(MapConverter.ToMapWithDefault(5, map)["name1"], "title 1");
            Assert.Equal(MapConverter.ToMapWithDefault(5, map)["name2"], "title 2");
        }
    }
}
