using Xunit;
using PipServices3.Commons.Validate;
using PipServices3.Commons.Data;

namespace PipServices3.Commons.Test.Validate
{
    //[TestClass]
    public class PagingParamsSchemaTest
    {
        [Fact]
        public void TestEmptyPagingParams()
        {
            var schema = new PagingParamsSchema();
            var pagingParams = new PagingParams();

            var results = schema.Validate(pagingParams);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void TestNonEmptyPagingParams()
        {
            var schema = new PagingParamsSchema();
            var pagingParams = new PagingParams(1, 1, true);

            var results = schema.Validate(pagingParams);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void TestOptionalPagingParams()
        {
            var schema = new PagingParamsSchema();
            var pagingParams = new PagingParams()
            {
                Skip = null,
                Take = null
            };

            var results = schema.Validate(pagingParams);
            Assert.Equal(0, results.Count);
        }
    }
}
