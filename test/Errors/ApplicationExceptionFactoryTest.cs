using System;
using PipServices3.Commons.Errors;
using PipServices3.Commons.Data;
using Xunit;

using ApplicationException = PipServices3.Commons.Errors.ApplicationException;

namespace PipServices3.Commons.Test.Errors
{
    //[TestClass]
    public class ApplicationExceptionFactoryTest
    {
        private ErrorDescription _descr = new ErrorDescription();

        public ApplicationExceptionFactoryTest()
        {
            _descr.CorrelationId = "correlationId";
            _descr.Code= "code";
            _descr.Message = "message";
            _descr.Status = 777;
            _descr.Cause = "cause";
            _descr.StackTrace = "stackTrace";

            var map = new StringValueMap();
            map["key"] = "value";
            _descr.Details = map;
        }

        [Fact]
        public void Create_FromUnknown_IsOk()
        {
            _descr.Category = ErrorCategory.Unknown;

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<UnknownException>(ex);
        }

        [Fact]
        public void Create_FromInternal_IsOk()
        {
            _descr.Category = ErrorCategory.Internal;

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<InternalException>(ex);
        }

        [Fact]
        public void Create_FromMisconfiguration_IsOk()
        {
            _descr.Category = ErrorCategory.Misconfiguration;

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<ConfigException>(ex);
        }

        [Fact]
        public void Create_FromNoResponse_IsOk()
        {
            _descr.Category = ErrorCategory.NoResponse;

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<ConnectionException>(ex);
        }

        [Fact]
        public void Create_FromFailedInvocation_IsOk()
        {
            _descr.Category = ErrorCategory.FailedInvocation;

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<InvocationException>(ex);
        }

        [Fact]
        public void Create_FromNoFileAccess_IsOk()
        {
            _descr.Category = ErrorCategory.NoFileAccess;

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<FileException>(ex);
        }

        [Fact]
        public void Create_FromBadRequest_IsOk()
        {
            _descr.Category = ErrorCategory.BadRequest;

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<BadRequestException>(ex);
        }

        [Fact]
        public void Create_FromUnauthorized_IsOk()
        {
            _descr.Category = ErrorCategory.Unauthorized;

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<UnauthorizedException>(ex);
        }

        [Fact]
        public void Create_FromConflict_IsOk()
        {
            _descr.Category = ErrorCategory.Conflict;

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<ConflictException>(ex);
        }

        [Fact]
        public void Create_FromNotFound_IsOk()
        {
            _descr.Category = ErrorCategory.NotFound;

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<NotFoundException>(ex);
        }

        [Fact]
        public void Create_FromUnsupported_IsOk()
        {
            _descr.Category = ErrorCategory.Unsupported;

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<UnsupportedException>(ex);
        }

        [Fact]
        public void Create_FromDefault_IsOk()
        {
            _descr.Category = "any_other";

            var ex = ApplicationExceptionFactory.Create(_descr);

            CheckProperties(ex);

            Assert.IsType<UnknownException>(ex);
            Assert.Equal(_descr.Category, ex.Category);
            Assert.Equal(_descr.Status, ex.Status);
        }

        private void CheckProperties(ApplicationException ex)
        {
            Assert.NotNull(ex);

            Assert.Equal(_descr.Cause, ex.Cause);
            Assert.Equal(_descr.StackTrace, ex.StackTrace);
            Assert.Equal(_descr.Details, ex.Details);
        }
    }
}
