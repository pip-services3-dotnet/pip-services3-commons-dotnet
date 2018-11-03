using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Class of errors related to access of missing objects.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class NotFoundException : ApplicationException
    {
        /// <summary>
        /// Creates an error instance with error message.
        /// </summary>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        [JsonConstructor]
        public NotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="innerException">an error object</param>
        /// See <see cref="ErrorCategory.NotFound"/>
        public NotFoundException(Exception innerException) 
            : base(ErrorCategory.NotFound, null, null, null)
        {
            Status = 404;
            WithCause(innerException);
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="correlationId">(optional) a unique transaction id to trace execution through call chain.</param>
        /// <param name="code">(optional) a unique error code. Default: "UNKNOWN"</param>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        /// <param name="innerException">an error object</param>
        public NotFoundException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.NotFound, correlationId, code, message)
        {
            Status = 404;
            WithCause(innerException);
        }

#if !CORE_NET
        protected NotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}