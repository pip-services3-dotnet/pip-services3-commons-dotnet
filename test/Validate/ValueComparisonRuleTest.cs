using Xunit;
using PipServices3.Commons.Validate;

namespace PipServices3.Commons.Test.Validate
{
    //[TestClass]
    public class ValueComparisonRuleTest
    {
        [Fact]
        public void TestEqualComparison()
        {
            var schema = new Schema().WithRule(new ValueComparisonRule("EQ", 123));
            var results = schema.Validate(123);
            Assert.Empty(results);

            results = schema.Validate(432);
            Assert.Single(results);

            schema = new Schema().WithRule(new ValueComparisonRule("EQ", "ABC"));
            results = schema.Validate("ABC");
            Assert.Empty(results);
        }

        [Fact]
        public void TestNotEqualComparison()
        {
            var schema = new Schema().WithRule(new ValueComparisonRule("NE", 123));
            var results = schema.Validate(123);
            Assert.Single(results);

            results = schema.Validate(432);
            Assert.Empty(results);

            schema = new Schema().WithRule(new ValueComparisonRule("NE", "ABC"));
            results = schema.Validate("XYZ");
            Assert.Empty(results);
        }

        [Fact]
        public void TestLessComparison()
        {
            var schema = new Schema().WithRule(new ValueComparisonRule("LE", 123));
            var results = schema.Validate(123);
            Assert.Empty(results);

            results = schema.Validate(432);
            Assert.Single(results);

            schema = new Schema().WithRule(new ValueComparisonRule("LT", 123));
            results = schema.Validate(123);
            Assert.Single(results);
        }

        [Fact]
        public void TestMoreComparison()
        {
            var schema = new Schema().WithRule(new ValueComparisonRule("GE", 123));
            var results = schema.Validate(123);
            Assert.Empty(results);

            results = schema.Validate(432);
            Assert.Empty(results);

            schema = new Schema().WithRule(new ValueComparisonRule("GT", 123));
            results = schema.Validate(123);
            Assert.Single(results);
        }

        [Fact]
        public void TestMatchComparison()
        {
            var schema = new Schema().WithRule(new ValueComparisonRule("LIKE", "A.*"));
            var results = schema.Validate("ABC");
            Assert.Empty(results);

            results = schema.Validate("XYZ");
            Assert.Single(results);
        }
    }
}
