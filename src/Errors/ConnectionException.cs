using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Errors that occur during connections to remote services.
    /// They can be related to misconfiguration, network issues, or the remote service itself.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class ConnectionException : ApplicationException
    {
        /// <summary>
        /// Creates an error instance with error message.
        /// </summary>
        /// <param name="message">a human-readable description of the error.</param>
        [JsonConstructor]
        public ConnectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an error instance with noresponse error category and assigns its values.
        /// </summary>
        /// <param name="innerException">an error object</param>
        public ConnectionException(Exception innerException) 
            : base(ErrorCategory.NoResponse, null, null, null)
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
        public ConnectionException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.NoResponse, correlationId, code, message)
        {
            Status = 500;
            WithCause(innerException);
        }

#if !CORE_NET
        protected ConnectionException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}