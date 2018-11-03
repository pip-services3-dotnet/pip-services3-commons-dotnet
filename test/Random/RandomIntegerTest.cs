using Xunit;

namespace PipServices3.Commons.Random
{
    //[TestClass]
    public class RandomIntegerTest
    {
        [Fact]
        public void TestNextInteger()
        {
            float value = RandomInteger.NextInteger(5);
            Assert.True(value <= 5);

            value = RandomInteger.NextInteger(2, 5);
            Assert.True(value <= 5 && value >= 2);
        }

        [Fact]
        public void TestUpdateInteger()
        {
            float value = RandomInteger.UpdateInteger(0, 5);
            Assert.True(value <= 5 && value >= -5);

            value = RandomInteger.UpdateInteger(5, 0);

            value = RandomInteger.UpdateInteger(0);
            Assert.True(value == 0);
        }

        [Fact]
        public void TestSequence()
        {
            var list = RandomInteger.Sequence(1, 5);
            Assert.True(list.Count <= 5 && list.Count >= 1);

            list = RandomInteger.Sequence(-1, 0);
            Assert.True(list.Count == 0);

            list = RandomInteger.Sequence(-1, -4);
            Assert.True(list.Count == 0);

            list = RandomInteger.Sequence(4, 4);
            Assert.True(list.Count == 4);

            list = RandomInteger.Sequence(5);
            Assert.True(list.Count == 5);
        }

    }
}
