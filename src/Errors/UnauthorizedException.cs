using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices.Commons.Errors
{
    /// <summary>
    /// Class of errors related to unauthorized access to secured objects or services.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class UnauthorizedException : ApplicationException
    {
        [JsonConstructor]
        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(Exception innerException) 
            : base(ErrorCategory.Unauthorized, null, null, null)
        {
            Status = 401;
            WithCause(innerException);
        }

        public UnauthorizedException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.Unauthorized, correlationId, code, message)
        {
            Status = 401;
            WithCause(innerException);
        }

#if !CORE_NET
        protected UnauthorizedException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}