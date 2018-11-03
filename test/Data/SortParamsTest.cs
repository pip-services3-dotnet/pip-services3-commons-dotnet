using PipServices3.Commons.Convert;
using PipServices3.Commons.Data;
using PipServices3.Commons.Run;

using System.Collections.Generic;

using Xunit;

namespace PipServices3.Commons.Test.Data
{
    public sealed class SortParamsTest
    {
        [Fact]
        public void It_Should_Create_Sort_Params_From_Null_Object()
        {
            var parameters = SortParams.FromValue(null);

            Assert.Empty(parameters);
        }

        [Fact]
        public void It_Should_Create_Sort_Params_From_Array_Of_Sort_Fields()
        {
            var parameters = SortParams.FromValue(new List<object>
            {
                new SortField("field1"),
                new SortField("field2", false),
                new SortField("field3", false)
            });

            Assert.Equal(3, parameters.Count);
            Assert.Equal("field1", parameters[0].Name);
            Assert.Equal("field2", parameters[1].Name);
            Assert.Equal("field3", parameters[2].Name);
        }

        [Fact]
        public void It_Should_Create_Sort_Params_From_Object()
        {
            var sortParams = new SortParams()
            {
                new SortField("field1"),
                new SortField("field2", false),
                new SortField("field3", false)
            };

            var httpClientSortParams = new
            {
                sort = sortParams
            };

            var jsonSortParams = JsonConverter.ToJson(httpClientSortParams);

            var parameters = Parameters.FromJson(jsonSortParams);
            var sort = SortParams.FromValue(parameters.Get("sort"));

            Assert.Equal(3, sort.Count);
            Assert.Equal(sortParams[0].Name, sort[0].Name);
            Assert.Equal(sortParams[1].Name, sort[1].Name);
            Assert.Equal(sortParams[2].Name, sort[2].Name);
        }
    }
}
