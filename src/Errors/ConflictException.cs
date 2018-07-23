using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices.Commons.Errors
{
    /// <summary>
    /// Class of errors related to conflict in object versions between the user request and the server.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class ConflictException : ApplicationException
    {
        [JsonConstructor]
        public ConflictException(string message)
            : base(message)
        {
        }

        public ConflictException(Exception innerException) 
            : base(ErrorCategory.Conflict, null, null, null)
        {
            Status = 409;
            WithCause(innerException);
        }

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