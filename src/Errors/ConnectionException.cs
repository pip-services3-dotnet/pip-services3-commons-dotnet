using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices.Commons.Errors
{
    /// <summary>
    /// Errors happened during connection to remote services.
    /// They can be related to misconfiguration, network issues or remote service itself
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class ConnectionException : ApplicationException
    {
        [JsonConstructor]
        public ConnectionException(string message)
            : base(message)
        {
        }

        public ConnectionException(Exception innerException) 
            : base(ErrorCategory.NoResponse, null, null, null)
        {
            Status = 500;
            WithCause(innerException);
        }

        public ConnectionException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.NoResponse, correlationId, code, message)
        {
            Status = 500;
            WithCause(innerException);
        }

#if !CORE_NET
        protected ConnectionException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}