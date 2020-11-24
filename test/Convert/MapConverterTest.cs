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
            Assert.Equal(2, MapConverter.ToNullableMap(array).Count);
            Assert.True(MapConverter.ToNullableMap(array).ContainsKey("0"));
            Assert.True(MapConverter.ToNullableMap(array).ContainsKey("1"));
            Assert.Equal(1, MapConverter.ToNullableMap(array)["0"]);
            Assert.Equal(2, MapConverter.ToNullableMap(array)["1"]);
            Assert.True(MapConverter.ToNullableMap(array).Values.Contains(1));
            Assert.True(MapConverter.ToNullableMap(array).Values.Contains(2));

            String[] values = { "ab", "cd" };
            Assert.Equal(2, MapConverter.ToNullableMap(values).Count);
            Assert.True(MapConverter.ToNullableMap(values).ContainsKey("0"));
            Assert.True(MapConverter.ToNullableMap(values).ContainsKey("1"));
            Assert.Equal("ab", MapConverter.ToNullableMap(values)["0"]);
            Assert.Equal("cd", MapConverter.ToNullableMap(values)["1"]);
            Assert.True(MapConverter.ToNullableMap(values).Values.Contains("ab"));
            Assert.True(MapConverter.ToNullableMap(values).Values.Contains("cd"));

            Dictionary<int, object> map = new Dictionary<int, object>();
            map.Add(8, "title 8");
            map.Add(11, "title 11");
            Assert.Equal(2, MapConverter.ToNullableMap(map).Count);
            Assert.True(MapConverter.ToNullableMap(map).ContainsKey("8"));
            Assert.True(MapConverter.ToNullableMap(map).ContainsKey("11"));
            Assert.Equal("title 8", MapConverter.ToNullableMap(map)["8"]);
            Assert.Equal("title 11", MapConverter.ToNullableMap(map)["11"]);
            Assert.True(MapConverter.ToNullableMap(map).Values.Contains("title 8"));
            Assert.True(MapConverter.ToNullableMap(map).Values.Contains("title 11"));
        }

        [Fact]
        public void TestToMap()
        {
            Assert.Equal(0, MapConverter.ToMap(null).Count);
            Assert.Equal(0, MapConverter.ToMap(5).Count);
        }

        [Fact]
        public void TestToMapRecursive()
        {
            dynamic dummy = new
            {
                Id = 1,
                Name = "name",
                Dummy = new
                {
                    Type = "dummy type",
                    ArrayOfDouble = new double[] { 10.3, 10.2 }
                }
            };

            IDictionary<string, object> map = MapConverter.ToMap(dummy);
            Assert.Equal(3, map.Keys.Count);
            Assert.True(map.ContainsKey("Id"));
            Assert.True(map.ContainsKey("Name"));
            Assert.True(map.ContainsKey("Dummy"));
            Assert.Equal(dummy.Id, map["Id"]);
            Assert.Equal(dummy.Name, map["Name"]);
            Assert.NotNull(map["Dummy"]);

            IDictionary<string, object> mapDummy = map["Dummy"] as IDictionary<string, object>;
            Assert.NotNull(mapDummy);
            Assert.Equal(2, mapDummy.Keys.Count);
            Assert.True(mapDummy.ContainsKey("Type"));
            Assert.True(mapDummy.ContainsKey("ArrayOfDouble"));
            Assert.Equal(dummy.Dummy.Type, mapDummy["Type"]);
            Assert.Equal(dummy.Dummy.ArrayOfDouble, mapDummy["ArrayOfDouble"]);
        }

        [Fact]
        public void TestToMapWithDefault()
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add("name1", "title 1");
            map.Add("name2", "title 2");
            Assert.Equal(2, MapConverter.ToMapWithDefault(null, map).Count);
            Assert.Equal("title 1", MapConverter.ToMapWithDefault(null, map)["name1"]);
            Assert.Equal("title 2", MapConverter.ToMapWithDefault(null, map)["name2"]);

            Assert.Equal(2, MapConverter.ToMapWithDefault(5, map).Count);
            Assert.Equal("title 1", MapConverter.ToMapWithDefault(5, map)["name1"]);
            Assert.Equal("title 2", MapConverter.ToMapWithDefault(5, map)["name2"]);
        }
    }
}
