using System.Collections.Generic;

namespace PipServices.Commons.Test.Validate
{
    public class TestObject
    {
        public TestObject() { }

        private float privateField = 124;
        private string PrivateProperty { get; set; } = "XYZ";
        
        public int IntField = 12345;
        public string StringProperty { get; set; } = "ABC";
        public object NullProperty { get; set; } = null;
        public int[] IntArrayProperty { get; set; } = new int[] { 1, 2, 3 };
        public List<string> StringListProperty { get; set; } = new List<string>(new string[] { "AAA", "BBB" });
        public Dictionary<string, int> MapProperty = new Dictionary<string, int>() { { "Key1", 111 }, { "Key2", 222 } };
        public TestSubObject SubObjectProperty { get; set; } = new TestSubObject("1");
        public TestSubObject[] SubArrayProperty { get; set; } = new TestSubObject[] { new TestSubObject("2"), new TestSubObject("3") };
    }
}
