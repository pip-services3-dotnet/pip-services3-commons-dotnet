using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class RecursiveMapConverterTest
    {
        class TestClass
        {
            public TestClass(object value1, object value2)
            {
                this.value1 = value1;
                this.value2 = value2;
            }

            public object value1;
            public object value2;
        }

        //[Fact]
        public void TestObjectToMap()
        {
            // Handling nulls
            object value = null;
            IDictionary<string, object> result = RecursiveMapConverter.ToNullableMap(value);
            Assert.Null(result);

            // Handling simple objects
            //value = new TestClass(123, 234);
            //result = RecursiveMapConverter.ToNullableMap(value);
            //Assert.Equal(123, result["value1"]);
            //Assert.Equal(234, result["value2"]);

            // Handling dictionaries
            value = new Dictionary<string, object>();
            result = RecursiveMapConverter.ToNullableMap(value);
            Assert.Equal(value, result);

            // Non-recursive conversion
            //        value = new TestClass(123, new TestClass(111, 222));
            //        result = RecursiveMapConverter.toMap(value, null, false);
            //        assertNotNull(result);
            //        assertEquals(123, result.get("value1"));
            //        assertNotNull(result.get("value2"));
            //        assertFalse(result.get("value2") instanceof Map<?,?>);
            //        assertTrue(result.get("value2") instanceof TestClass);

            //// Recursive conversion
            //value = new TestClass(123, new TestClass(111, 222));
            //result = RecursiveMapConverter.ToNullableMap(value);
            //Assert.NotNull(result);
            //Assert.Equal(123, result["value1"]);
            //Assert.NotNull(result["value2"]);
            //Assert.True(result["value2"] is IDictionary);

            //// Handling arrays
            //value = new TestClass(new Object[] { new TestClass(111, 222) }, null);
            //result = RecursiveMapConverter.ToNullableMap(value);
            //Assert.NotNull(result);
            //Assert.True(result["value1"] is IList);
            //List<object> resultElements = ((List<object>)result["value1"]);
            //Dictionary<string, object> resultElement0 = (Dictionary<string, object>)resultElements[0];
            //Assert.NotNull(resultElement0);
            //Assert.Equal(111, resultElement0["value1"]);
            //Assert.Equal(222, resultElement0["value2"]);

            //// Handling lists
            //value = new TestClass(new List<object>(new object[] { new TestClass(111, 222) }), null);
            //result = RecursiveMapConverter.ToNullableMap(value);
            //Assert.NotNull(result);
            //Assert.True(result["value1"] is IList);
            //resultElements = ((List<object>)result["value1"]);
            //resultElement0 = (Dictionary<string, object>)resultElements[0];
            //Assert.NotNull(resultElement0);
            //Assert.Equal(111, resultElement0["value1"]);
            //Assert.Equal(222, resultElement0["value2"]);
        }

    }
}
