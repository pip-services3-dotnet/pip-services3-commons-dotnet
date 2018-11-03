using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Errors raised by conflicts between object versions that were
    /// posted by the user and those that are stored on the server.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class ConflictException : ApplicationException
    {
        /// <summary>
        /// Creates an error instance with error message
        /// </summary>
        /// <param name="message">a human-readable description of the error.</param>
        [JsonConstructor]
        public ConflictException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an error instance with conflict error category and assigns its values.
        /// </summary>
        /// <param name="innerException">an error object</param>
        /// See <see cref="ErrorCategory.Conflict"/>
        public ConflictException(Exception innerException) 
            : base(ErrorCategory.Conflict, null, null, null)
        {
            Status = 409;
            WithCause(innerException);
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="correlationId">(optional) a unique transaction id to trace execution through call chain.</param>
        /// <param name="code">(optional) a unique error code. Default: "UNKNOWN"</param>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        /// <param name="innerException">an error object</param>
        public ConflictException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.Conflict, correlationId, code, message)
        {
            Status = 409;
            WithCause(innerException);
        }

#if !CORE_NET
        protected ConflictException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}