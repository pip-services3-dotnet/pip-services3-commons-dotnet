using PipServices3.Commons.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;

using Xunit;

namespace PipServices3.Commons.Test.Data
{
    public sealed class ProjectionExtensionsTest
    {
        [Fact]
        public void It_Should_Return_Projected_Fields_For_Object()
        {
            dynamic obj = CreateExpandoObject();
                
            var projectionParams = ProjectionParams.FromValues("Id,Name");

            dynamic result = projectionParams.FromObject(obj as ExpandoObject);

            Assert.Equal(obj.Id, result.Id);
            Assert.Equal(obj.Name, result.Name);

            AssertProperties(result, "Id", "Name");
        }

        [Fact]
        public void It_Should_Return_Projected_Fields_For_Object_With_Nested_Properties()
        {
            dynamic obj = CreateExpandoObject();

            var projectionParams = ProjectionParams.FromValues("Id,Description,Data(Id,Amount)");

            dynamic result = projectionParams.FromObject(obj as ExpandoObject);

            Assert.Equal(obj.Id, result.Id);
            Assert.Equal(obj.Description, result.Description);
            Assert.Equal(obj.Data.Id, result.Data.Id);
            Assert.Equal(obj.Data.Amount, result.Data.Amount);

            AssertProperties(result, "Id", "Description", "Data");
            AssertProperties(result.Data, "Id", "Amount");
        }

        [Fact]
        public void It_Should_Return_Projected_Fields_For_Object_With_Nested_Collections()
        {
            dynamic obj = CreateExpandoObject();

            var projectionParams = ProjectionParams.FromValues("Id,Description,Data(Id,Amount),DataList(Memo)");

            dynamic result = projectionParams.FromObject(obj as ExpandoObject);

            Assert.Equal(obj.Id, result.Id);
            Assert.Equal(obj.Description, result.Description);
            Assert.Equal(obj.Data.Id, result.Data.Id);
            Assert.Equal(obj.Data.Amount, result.Data.Amount);
            Assert.Equal(obj.DataList[0].Memo, result.DataList[0].Memo);
            Assert.Equal(obj.DataList[1].Memo, result.DataList[1].Memo);

            AssertProperties(result, "Id", "Description", "Data", "DataList");
            AssertProperties(result.Data, "Id", "Amount");
            AssertProperties(result.DataList[0], "Memo");
            AssertProperties(result.DataList[1], "Memo");
        }

        private dynamic CreateExpandoObject()
        {
            dynamic obj = new ExpandoObject();

            obj.Id = "1";
            obj.Name = "Test Name";
            obj.Description = "Test Description";

            // nested data
            obj.Data = new ExpandoObject();
            obj.Data.Id = "Data 1";
            obj.Data.Amount = 10;
            obj.Data.Title = "Money";

            // nested collection
            obj.DataList = new List<object>();

            dynamic dataListItem1 = new ExpandoObject();
            dataListItem1.Id = "Data List Item 1";
            dataListItem1.Memo = "Data List Item Memo 1";

            dynamic dataListItem2 = new ExpandoObject();
            dataListItem2.Id = "Data List Item 2";
            dataListItem2.Memo = "Data List Item Memo 2";

            obj.DataList.Add(dataListItem1);
            obj.DataList.Add(dataListItem2);

            return obj;
        }

        private void AssertProperties(ExpandoObject @object, params string[] propertyNames)
        {
            foreach (var keyValue in @object)
            {
                Assert.Contains(keyValue.Key, propertyNames);
            }

            foreach (var propertyName in propertyNames)
            {
                Assert.Contains(propertyName, ((IDictionary<string, object>)@object).Keys);
            }
        }
    }
}
