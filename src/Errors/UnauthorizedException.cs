using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Access errors caused by missing user identity (authentication error) or incorrect security permissions (authorization error).
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class UnauthorizedException : ApplicationException
    {
        /// <summary>
        /// Creates an error instance with error message.
        /// </summary>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        [JsonConstructor]
        public UnauthorizedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="innerException">an error object</param>
        /// See <see cref="ErrorCategory.Unauthorized"/>
        public UnauthorizedException(Exception innerException) 
            : base(ErrorCategory.Unauthorized, null, null, null)
        {
            Status = 401;
            WithCause(innerException);
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="correlationId">(optional) a unique transaction id to trace execution through call chain.</param>
        /// <param name="code">(optional) a unique error code. Default: "UNKNOWN"</param>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        /// <param name="innerException">an error object</param>
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