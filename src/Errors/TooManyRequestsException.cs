using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Errors caused by too many requests
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class TooManyRequestsException : ApplicationException
    {
        /// <summary>
        /// Creates an error instance with error message.
        /// </summary>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        [JsonConstructor]
        public TooManyRequestsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="innerException">an error object</param>
        /// See <see cref="ErrorCategory.TooManyRequests"/>
        public TooManyRequestsException(Exception innerException) 
            : base(ErrorCategory.TooManyRequests, null, null, null)
        {
            Status = 429;
            WithCause(innerException);
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="correlationId">(optional) a unique transaction id to trace execution through call chain.</param>
        /// <param name="code">(optional) a unique error code. Default: "UNKNOWN"</param>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        /// <param name="innerException">an error object</param>
        public TooManyRequestsException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.TooManyRequests, correlationId, code, message)
        {
            Status = 429;
            WithCause(innerException);
        }

#if !CORE_NET
        protected TooManyRequestsException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}