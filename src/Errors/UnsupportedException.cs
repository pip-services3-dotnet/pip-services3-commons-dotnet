using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Errors caused by calls to unsupported or not yet implemented functionality
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class UnsupportedException : ApplicationException
    {
        /// <summary>
        /// Creates an error instance with error message.
        /// </summary>
        /// <param name="message">(optional) a human-readable description of the error.</param>

        [JsonConstructor]
        public UnsupportedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="innerException">an error object</param>
        /// See <see cref="ErrorCategory.Unsupported"/>
        public UnsupportedException(Exception innerException) 
            : base(ErrorCategory.Unsupported, null, null, null)
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
        public UnsupportedException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.Unsupported, correlationId, code, message)
        {
            Status = 500;
            WithCause(innerException);
        }

#if !CORE_NET
        protected UnsupportedException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}