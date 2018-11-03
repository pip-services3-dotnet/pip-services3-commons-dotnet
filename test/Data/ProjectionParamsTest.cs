using PipServices3.Commons.Data;

using System.Collections.Generic;

using Xunit;

namespace PipServices3.Commons.Test.Data
{
    public sealed class ProjectionParamsTest
    {
        [Fact]
        public void It_Should_Create_Projection_Params_From_Null_Object()
        {
            var parameters = ProjectionParams.FromValue(null);

            Assert.Empty(parameters);
        }

        [Fact]
        public void It_Should_Create_Projection_Params_From_Object()
        {
            var parameters = ProjectionParams.FromValue(new List<object> { "field1", "field2", "field3" });

            Assert.Equal(3, parameters.Count);
            Assert.Equal("field1", parameters[0]);
            Assert.Equal("field2", parameters[1]);
            Assert.Equal("field3", parameters[2]);
        }

        [Fact]
        public void It_Should_Convert_Parameters_From_Values()
        {
            var parameters = ProjectionParams.FromValues("field1", "field2", "field3");

            Assert.Equal(3, parameters.Count);
            Assert.Equal("field1", parameters[0]);
            Assert.Equal("field2", parameters[1]);
            Assert.Equal("field3", parameters[2]);
        }

        [Fact]
        public void It_Should_Convert_Parameters_From_Values_As_One_String()
        {
            var parameters = ProjectionParams.FromValues("field1,field2, field3");

            Assert.Equal(3, parameters.Count);
            Assert.Equal("field1", parameters[0]);
            Assert.Equal("field2", parameters[1]);
            Assert.Equal("field3", parameters[2]);
        }

        [Fact]
        public void It_Should_Convert_Parameters_From_Grouped_Values()
        {
            var parameters = ProjectionParams.FromValues("object1(field1)", "object2(field1, field2)", "field3");

            Assert.Equal(4, parameters.Count);
            Assert.Equal("object1.field1", parameters[0]);
            Assert.Equal("object2.field1", parameters[1]);
            Assert.Equal("object2.field2", parameters[2]);
            Assert.Equal("field3", parameters[3]);
        }

        [Fact]
        public void It_Should_Convert_Parameters_From_Grouped_Values_As_One_String()
        {
            var parameters = ProjectionParams.FromValues("object1(object2(field1,field2,object3(field1)))");

            Assert.Equal(3, parameters.Count);
            Assert.Equal("object1.object2.field1", parameters[0]);
            Assert.Equal("object1.object2.field2", parameters[1]);
            Assert.Equal("object1.object2.object3.field1", parameters[2]);
        }

        [Fact]
        public void It_Should_Convert_Parameters_From_Multiple_Grouped_Values1()
        {
            var parameters = ProjectionParams.FromValues("object1(field1, object2(field1, field2, field3, field4), field3)", "field2");

            Assert.Equal(7, parameters.Count);
            Assert.Equal("object1.field1", parameters[0]);
            Assert.Equal("object1.object2.field1", parameters[1]);
            Assert.Equal("object1.object2.field2", parameters[2]);
            Assert.Equal("object1.object2.field3", parameters[3]);
            Assert.Equal("object1.object2.field4", parameters[4]);
            Assert.Equal("object1.field3", parameters[5]);
            Assert.Equal("field2", parameters[6]);
        }

        [Fact]
        public void It_Should_Convert_Parameters_From_Multiple_Grouped_Values2()
        {
            var parameters = ProjectionParams.FromValues("object1(field1, object2(field1), field3)", "field2");

            Assert.Equal(4, parameters.Count);
            Assert.Equal("object1.field1", parameters[0]);
            Assert.Equal("object1.object2.field1", parameters[1]);
            Assert.Equal("object1.field3", parameters[2]);
            Assert.Equal("field2", parameters[3]);
        }

        [Fact]
        public void It_Should_Convert_Parameters_From_Multiple_Grouped_Values3()
        {
            var parameters = ProjectionParams.FromValues("object1(field1, object2(field1, field2, object3(field1), field4), field3)", "field2");

            Assert.Equal(7, parameters.Count);
            Assert.Equal("object1.field1", parameters[0]);
            Assert.Equal("object1.object2.field1", parameters[1]);
            Assert.Equal("object1.object2.field2", parameters[2]);
            Assert.Equal("object1.object2.object3.field1", parameters[3]);
            Assert.Equal("object1.object2.field4", parameters[4]);
            Assert.Equal("object1.field3", parameters[5]);
            Assert.Equal("field2", parameters[6]);
        }

        [Fact]
        public void It_Should_Convert_Parameters_From_Multiple_Grouped_Values4()
        {
            var parameters = ProjectionParams.FromValues("object1(object2(object3(field1)), field2)", "field2");

            Assert.Equal(3, parameters.Count);
            Assert.Equal("object1.object2.object3.field1", parameters[0]);
            Assert.Equal("object1.field2", parameters[1]);
            Assert.Equal("field2", parameters[2]);
        }

        [Fact]
        public void It_Should_Convert_Parameters_From_Multiple_Grouped_Values5()
        {
            var parameters = ProjectionParams.FromValues("field1,object1(field1),object2(field1,field2),object3(field1),field2,field3");

            Assert.Equal(7, parameters.Count);
            Assert.Equal("field1", parameters[0]);
            Assert.Equal("object1.field1", parameters[1]);
            Assert.Equal("object2.field1", parameters[2]);
            Assert.Equal("object2.field2", parameters[3]);
            Assert.Equal("object3.field1", parameters[4]);
            Assert.Equal("field2", parameters[5]);
            Assert.Equal("field3", parameters[6]);
        }

        [Fact]
        public void It_Should_Convert_Parameters_From_Multiple_Grouped_Values_And_Another_Delimiter()
        {
            var parameters = ProjectionParams.FromValues(';', "field1;object1(field1);object2(field1;field2);object3(field1);field2;field3");

            Assert.Equal(7, parameters.Count);
            Assert.Equal("field1", parameters[0]);
            Assert.Equal("object1.field1", parameters[1]);
            Assert.Equal("object2.field1", parameters[2]);
            Assert.Equal("object2.field2", parameters[3]);
            Assert.Equal("object3.field1", parameters[4]);
            Assert.Equal("field2", parameters[5]);
            Assert.Equal("field3", parameters[6]);
        }

        [Fact]
        public void It_Should_Convert_Parameters_From_Multiple_Grouped_And_Delimitered_Values()
        {
            var parameters = ProjectionParams.FromValues(';', "A:abc,def;B:bcd;C(D:d,f,g;F:f)");

            Assert.Equal(4, parameters.Count);
            Assert.Equal("A:abc,def", parameters[0]);
            Assert.Equal("B:bcd", parameters[1]);
            Assert.Equal("C.D:d,f,g", parameters[2]);
            Assert.Equal("C.F:f", parameters[3]);
        }
    }
}
