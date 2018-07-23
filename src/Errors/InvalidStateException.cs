using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices.Commons.Errors
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
        [JsonConstructor]
        public InvalidStateException(string message)
            : base(message)
        {
        }

        public InvalidStateException(Exception innerException) 
            : base(ErrorCategory.InvalidState, null, null, null)
        {
            Status = 500;
            WithCause(innerException);
        }

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