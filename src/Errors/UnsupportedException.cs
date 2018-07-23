using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices.Commons.Errors
{
    /// <summary>
    /// Class of errors related to calls to unsupported functionality.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class UnsupportedException : ApplicationException
    {
        [JsonConstructor]
        public UnsupportedException(string message)
            : base(message)
        {
        }

        public UnsupportedException(Exception innerException) 
            : base(ErrorCategory.Unsupported, null, null, null)
        {
            Status = 500;
            WithCause(innerException);
        }

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