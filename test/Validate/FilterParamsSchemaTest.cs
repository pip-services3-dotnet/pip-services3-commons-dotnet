using Xunit;

using PipServices.Commons.Validate;
using PipServices.Commons.Data;

namespace PipServices.Commons.Test.Validate
{
    //[TestClass]
    public class FilterParamsSchemaTest
    {
        [Fact]
        public void TestEmptyFilterParamsSchema()
        {
            var schema = new FilterParamsSchema();
            var filterParams = new FilterParams();

            var results = schema.Validate(filterParams);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void TestNonEmptyFilterParamsSchema()
        {
            var schema = new FilterParamsSchema();
            var filterParams = new FilterParams()
            {
                {"key", "test"}
            };

            var results = schema.Validate(filterParams);
            Assert.Equal(0, results.Count);
        }
    }
}
