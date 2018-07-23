using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices.Commons.Errors
{
    /// <summary>
    /// Class of errors related to remote service calls.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class InvocationException : ApplicationException
    {
        [JsonConstructor]
        public InvocationException(string message)
            : base(message)
        {
        }

        public InvocationException(Exception innerException) 
            : base(ErrorCategory.FailedInvocation, null, null, null)
        {
            Status = 500;
            WithCause(innerException);
        }

        public InvocationException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.FailedInvocation, correlationId, code, message)
        {
            Status = 500;
            WithCause(innerException);
        }

#if !CORE_NET
        protected InvocationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}