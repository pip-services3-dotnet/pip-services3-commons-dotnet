using System;

namespace PipServices.Commons.Errors
{
    public static class ApplicationExceptionFactory
    {
        public static ApplicationException Create(ErrorDescription description)
        {
            if (description == null)
                throw new ArgumentNullException(nameof(description));

            ApplicationException error;
            if (description.Category == ErrorCategory.Unknown)
                error = new UnknownException(description.CorrelationId, description.Code, description.Message);
            else if (description.Category == ErrorCategory.Internal)
                error = new InternalException(description.CorrelationId, description.Code, description.Message);
            else if (description.Category == ErrorCategory.Misconfiguration)
                error = new ConfigException(description.CorrelationId, description.Code, description.Message);
            else if (description.Category == ErrorCategory.NoResponse)
                error = new ConnectionException(description.CorrelationId, description.Code, description.Message);
            else if (description.Category == ErrorCategory.FailedInvocation)
                error = new InvocationException(description.CorrelationId, description.Code, description.Message);
            else if (description.Category == ErrorCategory.NoFileAccess)
                error = new FileException(description.CorrelationId, description.Code, description.Message);
            else if (description.Category == ErrorCategory.BadRequest)
                error = new BadRequestException(description.CorrelationId, description.Code, description.Message);
            else if (description.Category == ErrorCategory.Unauthorized)
                error = new UnauthorizedException(description.CorrelationId, description.Code, description.Message);
            else if (description.Category == ErrorCategory.Conflict)
                error = new ConflictException(description.CorrelationId, description.Code, description.Message);
            else if (description.Category == ErrorCategory.NotFound)
                error = new NotFoundException(description.CorrelationId, description.Code, description.Message);
            else if (description.Category == ErrorCategory.Unsupported)
                error = new UnsupportedException(description.CorrelationId, description.Code, description.Message);
            else
            {
                error = new UnknownException(description.CorrelationId, description.Code, description.Message)
                {
                    Category = description.Category,
                    Status = description.Status
                };
            }

            error.Cause = description.Cause;
            error.StackTrace = description.StackTrace;
            error.Details = description.Details;

            return error;
        }

        public static Exception CreateOriginal(ErrorDescription description)
        {
            if (description == null)
                throw new ArgumentNullException(nameof(description));

            if (description.Type != null)
            {
                try
                {
                    var type = Type.GetType(description.Type);
                    Exception exception = null;

                    try
                    {
                        exception = Activator.CreateInstance(type, description.Message) as Exception;
                    }
                    catch
                    {
                        // Ignore...
                    }

                    try
                    {
                        if (exception == null)
                            exception = Activator.CreateInstance(type, description.Message) as Exception;
                    }
                    catch
                    {
                        // Ignore...
                    }

                    if (!(exception is ApplicationException))
                        return exception;
                }
                catch
                {
                    // Ignore...
                }
            }

            return Create(description);
        }
    }
}