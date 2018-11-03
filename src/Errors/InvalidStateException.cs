using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Class of errors related to operations called in wrong component state.
    /// For example, business calls when the component is not ready.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class InvalidStateException : ApplicationException
    {
        /// <summary>
        /// Creates an error instance with error message.
        /// </summary>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        [JsonConstructor]
        public InvalidStateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="innerException">an error object</param>
        /// See <see cref="ErrorCategory.InvalidState"/>
        public InvalidStateException(Exception innerException) 
            : base(ErrorCategory.InvalidState, null, null, null)
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
        public InvalidStateException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.InvalidState, correlationId, code, message)
        {
            Status = 500;
            WithCause(innerException);
        }

#if !CORE_NET
        protected InvalidStateException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}