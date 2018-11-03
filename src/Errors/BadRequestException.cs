using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Errors due to improper user requests. 
    /// 
    /// For example: missing or incorrect parameters.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class BadRequestException : ApplicationException
    {
        /// <summary>
        /// Creates an error instance with error message.
        /// </summary>
        /// <param name="message">a human-readable description of the error.</param>
        [JsonConstructor]
        public BadRequestException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an error instance with bad request error category and assigns its values.
        /// </summary>
        /// <param name="innerException">an error object</param>
        /// See <see cref="ErrorCategory.BadRequest"/>
        public BadRequestException(Exception innerException) 
            : base(ErrorCategory.BadRequest, null, null, null)
        {
            Status = 400;
            WithCause(innerException);
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="correlationId">(optional) a unique transaction id to trace execution through call chain.</param>
        /// <param name="code">(optional) a unique error code. Default: "UNKNOWN"</param>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        /// <param name="innerException">an error object</param>
        /// See <see cref="ErrorCategory"/>
        public BadRequestException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.BadRequest, correlationId, code, message)
        {
            Status = 400;
            WithCause(innerException);
        }

#if !CORE_NET
        protected BadRequestException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}