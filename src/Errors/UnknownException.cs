using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices.Commons.Errors
{
    /// <summary>
    /// Class of errors related to unknown or unexpected errors.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class UnknownException : ApplicationException
    {
        [JsonConstructor]
        public UnknownException(string message)
            : base(message)
        {
        }

        public UnknownException(Exception innerException) 
            : base(ErrorCategory.Unknown, null, null, null)
        {
            Status = 500;
            WithCause(innerException);
        }

        public UnknownException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.Unknown, correlationId, code, message)
        {
            Status = 500;
            WithCause(innerException);
        }

#if !CORE_NET
        protected UnknownException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}