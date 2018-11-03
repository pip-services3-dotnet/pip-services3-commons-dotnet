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
            Assert.Equal(0, results.Count);

            results = schema.Validate("ABC");
            Assert.Equal(1, results.Count);
        }

        [Fact]
        public void TestExcludedRule()
        {
            var schema = new Schema().WithRule(new ExcludedRule("AAA", "BBB", "CCC", null));
            var results = schema.Validate("AAA");
            Assert.Equal(1, results.Count);

            results = schema.Validate("ABC");
            Assert.Equal(0, results.Count);
        }
    }
}
