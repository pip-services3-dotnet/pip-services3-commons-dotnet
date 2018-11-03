using Xunit;

namespace PipServices3.Commons.Random
{
    //[TestClass]
    public class RandomTextTest
    {
        [Fact]
        public void TestPhrase()
        {
            Assert.True(RandomText.Phrase(-1) == "");
            Assert.True(RandomText.Phrase(-1, -2) == "");
            Assert.True(RandomText.Phrase(-1, 0) == "");
            Assert.True(RandomText.Phrase(-2, -1) == "");

            string text = RandomText.Phrase(4);
            Assert.True(text.Length >= 4 && text.Length <= 10);
            text = RandomText.Phrase(4, 10);
            Assert.True(text.Length >= 4);
        }

        [Fact]
        public void TestName()
        {
            string text = RandomText.Name();
            Assert.True(text.IndexOf(" ") != -1);
        }

        [Fact]
        public void TestPhone()
        {
            string text = RandomText.Phone();
            Assert.True(text.IndexOf("(") != -1);
            Assert.True(text.IndexOf(")") != -1);
            Assert.True(text.IndexOf("-") != -1);
        }

        [Fact]
        public void TestEmail()
        {
            string text = RandomText.Email();
            Assert.True(text.IndexOf("@") != -1);
            Assert.True(text.IndexOf(".com") != -1);
        }
    }
}
