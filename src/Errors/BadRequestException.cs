using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices.Commons.Errors
{
    /// <summary>
    /// Class of errors due to improper user requests, such as missing or wrong parameters.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class BadRequestException : ApplicationException
    {
        [JsonConstructor]
        public BadRequestException(string message)
            : base(message)
        {
        }

        public BadRequestException(Exception innerException) 
            : base(ErrorCategory.BadRequest, null, null, null)
        {
            Status = 400;
            WithCause(innerException);
        }

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