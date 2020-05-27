using Xunit;
using PipServices3.Commons.Validate;

namespace PipServices3.Commons.Test.Validate
{
    //[TestClass]
    public class LogicalRulesTest
    {
        [Fact]
        public void TestOrRule()
        {
            var schema = new Schema().WithRule(
                new OrRule(
                    new ValueComparisonRule("=", 1),
                    new ValueComparisonRule("=", 2)
                )
            );
            var result = schema.Validate(-100);
            Assert.Equal(2, result.Count);

            result = schema.Validate(1);
            Assert.Empty(result);

            result = schema.Validate(200);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void TestAndRule()
        {
            Schema schema = new Schema().WithRule(
                new AndRule(
                    new ValueComparisonRule(">", 0),
                    new ValueComparisonRule("<", 200)
                )
            );
            var result = schema.Validate(-100);
            Assert.Single(result);

            result = schema.Validate(100);
            Assert.Empty(result);

            result = schema.Validate(200);
            Assert.Single(result);
        }
    }
}
