using Xunit;

namespace PipServices3.Commons.Random
{
    //[TestClass]
    public class RandomStringTest
    {
        private string symbols = "_,.:-/.[].{},#-!,$=%.+^.&*-() ";
        private string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string digits = "01234956789";

        [Fact]
        public void TestPick()
        {
            Assert.True(RandomString.Pick("") == '\0');
            char charVariable = RandomString.Pick(chars);
            Assert.True(chars.IndexOf(charVariable) != -1);

            string[] valuesEmpty = { };
            Assert.True(RandomString.Pick(valuesEmpty) == "");

            string[] values = { "ab", "cd" };
            string result = RandomString.Pick(values);
            Assert.True(result == "ab" || result == "cd");
        }

        [Fact]
        public void testDistort()
        {
            string value = RandomString.Distort("abc");
            Assert.True(value.Length == 3 || value.Length == 4);
            Assert.True(value.Substring(0, 3) == "Abc"
                 || value.Substring(0, 3).Equals("abc")
            );

            if (value.Length == 4)
                Assert.True(symbols.IndexOf(value.Substring(3)) != -1);
        }

        [Fact]
        public void TestNextAlphaChar()
        {
            Assert.True(chars.IndexOf(RandomString.NextAlphaChar()) != -1);
        }

        [Fact]
        public void TestNextString()
        {
            string value = RandomString.NextString(3, 5);
            Assert.True(value.Length <= 5 && value.Length >= 3);

            for (int i = 0; i < value.Length; i++)
            {
                Assert.True(chars.IndexOf(value[i]) != -1
                    || symbols.IndexOf(value[i]) != -1
                    || digits.IndexOf(value[i]) != -1
                );
            }
        }
    }
}
