using System.Collections.Generic;
using Xunit;

namespace PipServices.Commons.Random
{
    //[TestClass]
    public class RandomArrayTest
    {
        [Fact]
        public void TestPick()
        {
            int[] listEmpty = { };
            int value = RandomArray.Pick(listEmpty);
            Assert.True(value == 0);

            int[] array = { 1, 2 };
            value = RandomArray.Pick(array);
            Assert.True(value == 1 || value == 2);

            List<int> list = new List<int>();
            Assert.Equal(0, RandomArray.Pick(list));

            list.Add(1);
            list.Add(2);
            value = RandomArray.Pick(array);
            Assert.True(value == 1 || value == 2);
        }
    }
}
