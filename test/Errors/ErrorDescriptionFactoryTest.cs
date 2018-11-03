using System;
using PipServices3.Commons.Errors;
using Xunit;

using ApplicationException = PipServices3.Commons.Errors.ApplicationException;

namespace PipServices3.Commons.Test.Errors
{
    //[TestClass]
    public class ErrorDescriptionFactoryTest
    {
        [Fact]
        public void Create_FromApplicationException_IsOk()
        {
            var key = "key";
            var details = "details";

            var ex = new ApplicationException("category", "correlationId", "code", "message")
            {
                Status = 777,
                Cause = "cause",
                StackTrace = "stackTrace"
            };
            ex.WithDetails(key, details);

            var descr = ErrorDescriptionFactory.Create(ex);

            Assert.NotNull(descr);
            Assert.Equal(ex.Category, descr.Category);
            Assert.Equal(ex.CorrelationId, descr.CorrelationId);
            Assert.Equal(ex.Code, descr.Code);
            Assert.Equal(ex.Message, descr.Message);
            Assert.Equal(ex.Status, descr.Status);
            Assert.Equal(ex.Cause, descr.Cause);
            Assert.Equal(ex.StackTrace, descr.StackTrace);
            Assert.Equal(ex.Details, descr.Details);
        }

        [Fact]
        public void Create_FromException_IsOk()
        {
            var ex = new Exception("message");

            var descr = ErrorDescriptionFactory.Create(ex);

            Assert.NotNull(descr);
            Assert.Equal(ErrorCategory.Unknown, descr.Category);
            Assert.Equal("UNKNOWN", descr.Code);
            Assert.Equal(ex.Message, descr.Message);
            Assert.Equal(500, descr.Status);
            Assert.Equal(ex.StackTrace, descr.StackTrace);
            Assert.Null(descr.CorrelationId);

            ex = new Exception("message");
            const string correlation = "correlation1234";
            var withCorrelation = ErrorDescriptionFactory.Create(ex, correlation);

            Assert.NotNull(descr);
            Assert.Equal(ErrorCategory.Unknown, withCorrelation.Category);
            Assert.Equal("UNKNOWN", withCorrelation.Code);
            Assert.Equal(ex.Message, withCorrelation.Message);
            Assert.Equal(500, withCorrelation.Status);
            Assert.Equal(ex.StackTrace, withCorrelation.StackTrace);
            Assert.Equal(correlation, withCorrelation.CorrelationId);
        }
    }
}
