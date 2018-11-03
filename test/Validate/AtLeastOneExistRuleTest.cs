using Xunit;
using PipServices3.Commons.Validate;

namespace PipServices3.Commons.Test.Validate
{
    //[TestClass]
    public class AtLeastOneExistRuleTest
    {
        [Fact]
        public void TestOnlyOneExistRule()
        {
            var obj = new TestObject();
            var schema = new Schema().WithRule(new AtLeastOneExistsRule("MissingProperty", "StringProperty", "NullProperty"));
            var results = schema.Validate(obj);
            Assert.Equal(0, results.Count);

            schema = new Schema().WithRule(new AtLeastOneExistsRule("StringProperty", "NullProperty", "intField"));
            results = schema.Validate(obj);
            Assert.Equal(0, results.Count);

            schema = new Schema().WithRule(new AtLeastOneExistsRule("MissingProperty", "NullProperty"));
            results = schema.Validate(obj);
            Assert.Equal(1, results.Count);
        }
    }
}
