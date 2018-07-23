using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices.Commons.Errors
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
        [JsonConstructor]
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(Exception innerException) 
            : base(ErrorCategory.NotFound, null, null, null)
        {
            Status = 404;
            WithCause(innerException);
        }

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