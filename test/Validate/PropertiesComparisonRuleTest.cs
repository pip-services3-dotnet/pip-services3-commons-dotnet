using Xunit;
using PipServices3.Commons.Validate;

namespace PipServices3.Commons.Test.Validate
{
    //[TestClass]
    public class PropertiesComparisonRuleTest
    {
        [Fact]
        public void TestPropertiesComparison()
        {
            var obj = new TestObject();
            var schema = new Schema().WithRule(new PropertiesComparisonRule("StringProperty", "EQ", "NullProperty"));

            obj.StringProperty = "ABC";
            obj.NullProperty = "ABC";
            var results = schema.Validate(obj);
            Assert.Equal(0, results.Count);

            obj.NullProperty = "XYZ";
            results = schema.Validate(obj);
            Assert.Equal(1, results.Count);
        }
    }
}
