using System;
using PipServices.Commons.Data;
using Xunit;

namespace PipServices.Commons.Test.Data
{
    //[TestClass]
    public sealed class IdGeneratorTest
    {
        [Fact]
        public void TestNextShort()
        {
            var id1 = IdGenerator.NextShort();
            Assert.NotNull(id1);
            Assert.True(id1.Length >= 9);

            var id2 = IdGenerator.NextShort();
            Assert.NotNull(id2);
            Assert.True(id2.Length >= 9);
            Assert.NotEqual(id1, id2);
        }

        [Fact]
        public void TestNextLong()
        {
            var id1 = IdGenerator.NextLong();
            Assert.NotNull(id1);
            Assert.True(id1.Length >= 32);

            var id2 = IdGenerator.NextLong();
            Assert.NotNull(id2);
            Assert.True(id2.Length >= 32);
            Assert.NotEqual(id1, id2);
        }
    }
}
