using PipServices.Commons.Config;
using PipServices.Commons.Data;
using Xunit;

namespace PipServices.Commons.Test.Config
{
    //[TestClass]
    public sealed class ConfigParamsTest
    {
        [Fact]
        public void TestConfigSections()
        {
            var config = ConfigParams.FromTuples(
                "Section1.Key1", "Value1",
                "Section1.Key2", "Value2",
                "Section1.Key3", "Value3"
            );
            Assert.Equal(3, config.Count);
            Assert.Equal("Value1", config.Get("Section1.Key1"));
            Assert.Equal("Value2", config.Get("Section1.Key2"));
            Assert.Equal("Value3", config.Get("Section1.Key3"));
            Assert.Null(config.Get("Section1.Key4"));

            var section2 = ConfigParams.FromTuples(
                "Key1", "ValueA",
                "Key2", "ValueB"
            );
            config.AddSection("Section2", section2);
            Assert.Equal(5, config.Count);
            Assert.Equal("ValueA", config.Get("Section2.Key1"));
            Assert.Equal("ValueB", config.Get("Section2.Key2"));

            var section1 = config.GetSection("Section1");
            Assert.Equal(3, section1.Count);
            Assert.Equal("Value1", section1.Get("Key1"));
            Assert.Equal("Value2", section1.Get("Key2"));
            Assert.Equal("Value3", section1.Get("Key3"));
        }


        [Fact]
        public void TestConfigFromString()
        {
            var config = ConfigParams.FromString("Queue=TestQueue;Endpoint=sb://cvctestbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=K70UpCUXN1Q5RFykll6/gz4Et14iJrYFnGPlwiFBlow=");
            Assert.Equal(4, config.Count);
            Assert.Equal("TestQueue", config.Get("Queue"));
        }

        [Fact]
        public void TestConfigFromObject()
        {
            var value = AnyValueMap.FromTuples(
                "field1", ConfigParams.FromString("field11=123;field12=ABC"),
                "field2", AnyValueArray.FromValues(
                    123, "ABC", ConfigParams.FromString("field21=543;field22=XYZ")
                ),
                "field3", true
            );

            var config = ConfigParams.FromValue(value);
            Assert.Equal(7, config.Count);
            Assert.Equal(123, config.GetAsInteger("field1.field11"));
            Assert.Equal("ABC", config.GetAsString("field1.field12"));
            Assert.Equal(123, config.GetAsInteger("field2.0"));
            Assert.Equal("ABC", config.GetAsString("field2.1"));
            Assert.Equal(543, config.GetAsInteger("field2.2.field21"));
            Assert.Equal("XYZ", config.GetAsString("field2.2.field22"));
            Assert.Equal(true, config.GetAsBoolean("field3"));
        }
    }
}
