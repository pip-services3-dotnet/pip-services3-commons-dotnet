using System;
using System.Text;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Factory to create serializeable ErrorDescription from ApplicationException
    /// or from arbitrary errors.
    /// 
    /// The ErrorDescriptions are used to pass errors through the wire between microservices
    /// implemented in different languages.They allow to restore exceptions on the receiving side
    /// close to the original type and preserve additional information.
    /// </summary>
    /// See <see cref="ErrorDescription"/>, <see cref="ApplicationException"/>
    public static class ErrorDescriptionFactory
    {
        /// <summary>
        /// Creates a serializable ErrorDescription from error object.
        /// </summary>
        /// <param name="ex">an error object</param>
        /// <returns>a serializeable ErrorDescription object that describes the error.</returns>
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

        /// <summary>
        /// Creates a serializable ErrorDescription from throwable object with unknown error category.
        /// </summary>
        /// <param name="ex">an error object</param>
        /// <param name="correlationId">(optional) a unique transaction id to trace execution through call chain.</param>
        /// <returns>a serializeable ErrorDescription object that describes the error.</returns>
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
                CorrelationId = correlationId,
                Cause = ex.InnerException != null ? ComposeCause(ex.InnerException) : null
            };
        }

        private static string ComposeCause(Exception error)
        {
            var builder = new StringBuilder();

            while (error != null)
            {
                builder.Append(error.Message)
                    .Append(" StackTrace: ")
                    .Append(error.StackTrace);

                error = error.InnerException;
            }

            return builder.ToString();
        }
    }
}
