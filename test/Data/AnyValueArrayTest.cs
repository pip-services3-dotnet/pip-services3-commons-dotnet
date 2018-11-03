using System;
using System.Collections.Generic;
using PipServices3.Commons.Data;
using PipServices3.Commons.Convert;
using Xunit;

namespace PipServices3.Commons.Test.Data
{
    //[TestClass]
    public sealed class AnyValueArrayTest
    {
        [Fact]
        public void TestCreateValueArray()
        {
            var array = new AnyValueArray();
            Assert.Equal(0, array.Count);

            array = new AnyValueArray(new [] { 1, 2, 3 });
            Assert.Equal("1,2,3", array.ToString());
            Assert.Equal(3, array.Count);
            Assert.True(array.Contains(1));
            Assert.True(array.ContainsAs<LogLevel>(LogLevel.Error));

            var list = new [] { 1, 2, 3 };
            array = new AnyValueArray(list);
            Assert.Equal(3, array.Count);
            Assert.True(array.ContainsAs<TimeSpan>(TimeSpan.FromMilliseconds(2)));
        }
    }
}
