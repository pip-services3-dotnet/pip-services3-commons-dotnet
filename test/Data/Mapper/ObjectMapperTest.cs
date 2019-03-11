using System.Collections.Generic;
using PipServices3.Commons.Data.Mapper;
using Xunit;


namespace PipServices3.Commons.Test.Data.Mapper
{
    public sealed class ObjectMapperTest
    {
        internal sealed class PlainClassA
        {
            public int Property1 { get; set; }
            public string Property2 { get; set; }
        }

        internal sealed class PlainClassB
        {
            public int Property1 { get; set; }
            public string Property2 { get; set; }
            public long Property3 { get; set; }
        }

        internal sealed class ClassA
        {
            public int Property1 { get; set; }
            public string Property2 { get; set; }
            public IEnumerable<object> Property3 { get; set; }
            public PlainClassA Property4 { get; set; }
            public IEnumerable<PlainClassA> Property6 { get; set; }
            
            public Dictionary<string, string> Property7 { get; set; } = new Dictionary<string, string>();

            public ClassA()
            {
                //Target collection must be created 
                Property3 = new List<object>();
                Property6 = new List<PlainClassA>();
            }
        }

        internal sealed class ClassB
        {
            public int Property1 { get; set; }
            public string Property2 { get; set; }
            public IEnumerable<object> Property3 { get; set; }
            public PlainClassA Property4 { get; set; }
            public long Property5 { get; set; }
            public IEnumerable<PlainClassB> Property6 { get; set; }
            
            public Dictionary<string, string> Property7 { get; set; } = new Dictionary<string, string>();

            public ClassB()
            {
                //Target collection must be created 
                Property3 = new List<object>();
                Property6 = new List<PlainClassB>();
            }
        }

        internal sealed class ClassC
        {
            public ClassA Property1 { get; set; }
        }

        internal sealed class ClassD
        {
            public ClassB Property1 { get; set; }
        }

        [Fact]
        public void MapTo_MapsPlainObjectToWider()
        {   
            var objectA = new ClassA()
            {
                Property1 = 100,
                Property2 = "Property2",
                Property3 = new object[] {"1", 2, "3"}
            };

            var objectB = ObjectMapper.MapTo<ClassB>(objectA);

            Assert.NotSame(objectA, objectB);
            Assert.IsType<ClassB>(objectB);
            Assert.Equal(objectA.Property1, objectB.Property1);
            Assert.Equal(objectA.Property2, objectB.Property2);
            Assert.Equal(objectA.Property3, objectB.Property3);
            Assert.Null(objectB.Property4);

            Assert.Equal(0, objectB.Property5);
        }

        [Fact]
        public void MapTo_MapsPlainObjectToNarrower()
        {
            var objectB = new ClassB()
            {
                Property1 = 100,
                Property2 = "Property2",
                Property3 = new object[] { "1", 2, "3" },
                Property5 = 10
            };

            var objectA = ObjectMapper.MapTo<ClassA>(objectB);

            Assert.NotSame(objectB, objectA);
            Assert.IsType<ClassA>(objectA);
            Assert.Equal(objectA.Property1, objectB.Property1);
            Assert.Equal(objectA.Property2, objectB.Property2);
            Assert.Equal(objectA.Property3, objectB.Property3);
        }

        [Fact]
        public void It_Should_Map_Objects_With_Empty_Arrays()
        {
            var objectC = new ClassC()
            {
                Property1 = new ClassA()
                {
                    Property1 = 100,
                    Property2 = "Property2",
                }
            };

            var objectD = ObjectMapper.MapTo<ClassD>(objectC);

            Assert.NotSame(objectC, objectD);
            Assert.IsType<ClassD>(objectD);

            Assert.Equal(objectC.Property1.Property1, objectD.Property1.Property1);
            Assert.Equal(objectC.Property1.Property2, objectD.Property1.Property2);
            Assert.Equal(objectC.Property1.Property3, objectD.Property1.Property3);
        }
        
        [Fact]
        public void It_Should_Map_Dictionaries()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("en", "Text in English");
            
            var objectB = new ClassB()
            {

                    Property1 = 100,
                    Property2 = "Property2",
                    Property7 = dictionary
            };

            var objectA = ObjectMapper.MapTo<ClassA>(objectB);

            Assert.NotSame(objectB, objectA);
            Assert.IsType<ClassA>(objectA);

            Assert.Equal(objectB.Property1, objectA.Property1);
            Assert.Equal(objectB.Property2, objectA.Property2);
            Assert.Equal(objectB.Property7, objectA.Property7);
        }
    }
}
