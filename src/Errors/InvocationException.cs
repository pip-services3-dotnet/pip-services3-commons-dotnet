using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Errors returned by remote services or network during call attempts
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class InvocationException : ApplicationException
    {
        /// <summary>
        /// Creates an error instance with error message.
        /// </summary>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        [JsonConstructor]
        public InvocationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="innerException">an error object</param>
        /// See <see cref="ErrorCategory.FailedInvocation"/>
        public InvocationException(Exception innerException) 
            : base(ErrorCategory.FailedInvocation, null, null, null)
        {
            Status = 500;
            WithCause(innerException);
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="correlationId">(optional) a unique transaction id to trace execution through call chain.</param>
        /// <param name="code">(optional) a unique error code. Default: "UNKNOWN"</param>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        /// <param name="innerException">an error object</param>
        public InvocationException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.FailedInvocation, correlationId, code, message)
        {
            Status = 500;
            WithCause(innerException);
        }

#if !CORE_NET
        protected InvocationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}