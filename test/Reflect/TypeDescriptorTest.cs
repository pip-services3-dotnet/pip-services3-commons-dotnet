using System;
using Xunit;

namespace PipServices3.Commons.Reflect
{
    //[TestClass]
    public class TypeDescriptorTest
    {
        [Fact]
        public void TestFromString()
        {
            TypeDescriptor descriptor = TypeDescriptor.FromString(null);
            Assert.Null(descriptor);

            descriptor = TypeDescriptor.FromString("xxx,yyy");
            Assert.Equal("xxx", descriptor.Name);
            Assert.Equal("yyy", descriptor.Library);

            descriptor = TypeDescriptor.FromString("xxx");
            Assert.Equal("xxx", descriptor.Name);
            Assert.Null(descriptor.Library);

		    try {
                descriptor = TypeDescriptor.FromString("xxx,yyy,zzz");
                Assert.True(false, "Wrong descriptor shall raise an exception");
            } catch (Exception) {
                // Ok...
            }
        }
    }
}
