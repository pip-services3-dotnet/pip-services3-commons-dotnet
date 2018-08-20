using Xunit;

using PipServices.Commons.Validate;
using PipServices.Commons.Data;

namespace PipServices.Commons.Test.Validate
{
    public class SortParamsSchemaTest
    {
        [Fact]
        public void TestEmptySortParams()
        {
            var schema = new SortParamsSchema();
            var sortParams = new SortParams();

            var results = schema.Validate(sortParams);
            Assert.Empty(results);
        }

        [Fact]
        public void TestNonEmptySortParams()
        {
            var schema = new SortParamsSchema();
            var sortParams = new SortParams(new []{ new SortField("field1"), new SortField("field2", false) });

            var results = schema.Validate(sortParams);
            Assert.Empty(results);
        }

        [Fact]
        public void TestOptionalSortParams()
        {
            var schema = new SortParamsSchema();
            var sortParams = new SortParams(new[] { new SortField() });

            var results = schema.Validate(sortParams);
            Assert.Empty(results);
        }
    }
}
