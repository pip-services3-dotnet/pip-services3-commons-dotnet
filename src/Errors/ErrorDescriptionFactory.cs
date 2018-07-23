using System;

namespace PipServices.Commons.Errors
{
    public static class ErrorDescriptionFactory
    {
        public static ErrorDescription Create(ApplicationException ex)
        {
            return new ErrorDescription()
            {
                Type = ex.GetType().FullName,
                Category = ex.Category,
                Status = ex.Status,
                Code = ex.Code,
                Message = ex.Message,
                Details = ex.Details,
                CorrelationId = ex.CorrelationId,
                Cause = ex.Cause,
                StackTrace = ex.StackTrace
            };
        }

        public static ErrorDescription Create(Exception ex, string correlationId = null)
        {
            return new ErrorDescription()
            {
                Type = ex.GetType().FullName,
                Category = ErrorCategory.Unknown,
                Status = 500,
                Code = "UNKNOWN",
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                CorrelationId = correlationId
            };
        }
    }
}
