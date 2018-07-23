using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices.Commons.Errors
{
    /// <summary>
    /// Class of errors related to internal system errors, programming mistakes, etc.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class InternalException : ApplicationException
    {
        [JsonConstructor]
        public InternalException(string message)
            : base(message)
        {
        }

        public InternalException()
        {
        }

        public InternalException(Exception innerException) 
            : base(ErrorCategory.Internal, null, null, null)
        {
            Status = 500;
            WithCause(innerException);
        }

        public InternalException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.Internal, correlationId, code, message)
        {
            Status = 500;
            WithCause(innerException);
        }

#if !CORE_NET
        protected InternalException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}