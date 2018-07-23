using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PipServices.Commons.Errors
{
    /// <summary>
    /// Class of errors related to read/write file operations.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class FileException : ApplicationException
    {
        [JsonConstructor]
        public FileException(string message)
            : base(message)
        {
        }
        
        public FileException(Exception innerException) 
            : base(ErrorCategory.NoFileAccess, null, null, null)
        {
            Status = 500;
            WithCause(innerException);
        }

        public FileException(string correlationId = null, string code = null, string message = null, Exception innerException = null) 
            : base(ErrorCategory.NoFileAccess, correlationId, code, message)
        {
            Status = 500;
            WithCause(innerException);
        }

#if !CORE_NET
        protected FileException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}