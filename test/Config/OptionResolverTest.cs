using PipServices3.Commons.Config;
using Xunit;

namespace PipServices3.Commons.Test.Config
{
    public class OptionResolverTest
    {
        [Fact]
        public void TestNormalOptionsResolution()
        {
            var config = ConfigParams.FromTuples("options.max_size", 1024);
            var options = OptionsResolver.Resolve(config);
            Assert.Equal(1024, options.GetAsInteger("max_size"));
        }

        [Fact]
        public void TestConfigsWithoutOptions()
        {
            var config = ConfigParams.FromTuples("name", "ABC");
            var options = OptionsResolver.Resolve(config);
            Assert.Equal(config, options);
        }
        
        [Fact]
        public void TestConfigsWithoutOptionsAndConfigAsDefault()
        {
            var config = ConfigParams.FromTuples("name", "ABC");
            var options = OptionsResolver.Resolve(config, false);
            Assert.Null(options);
        }
    }
}