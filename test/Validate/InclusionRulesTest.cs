using Xunit;
using PipServices3.Commons.Validate;

namespace PipServices3.Commons.Test.Validate
{
    //[TestClass]
    public class InclusionRulesTest
    {
        [Fact]
        public void TestIncludedRule()
        {
            var schema = new Schema().WithRule(new IncludedRule("AAA", "BBB", "CCC", null));
            var results = schema.Validate("AAA");
            Assert.Empty(results);

            results = schema.Validate("ABC");
            Assert.Single(results);
        }

        [Fact]
        public void TestExcludedRule()
        {
            var schema = new Schema().WithRule(new ExcludedRule("AAA", "BBB", "CCC", null));
            var results = schema.Validate("AAA");
            Assert.Single(results);

            results = schema.Validate("ABC");
            Assert.Empty(results);
        }
    }
}
