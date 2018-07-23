using PipServices.Commons.Errors;
using System;
using System.Runtime.Serialization;

namespace PipServices.Commons.Refer
{
    /// <summary>
    /// Exception thrown when required component is not found in references
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class ReferenceException : InternalException
    {
        public ReferenceException()
            : this(null, null)
        {
        }

        public ReferenceException(object locator)
            : base(null, "REF_ERROR", "Failed to obtain reference to " + locator)
        {
            WithDetails("locator", locator);
        }

        public ReferenceException(string correlationId, object locator)
            : base(correlationId, "REF_ERROR", "Failed to obtain reference to " + locator)
        {
            WithDetails("locator", locator);
        }

        public ReferenceException(string correlationId, string message)
            : base(correlationId, "REF_ERROR", message)
        { }

        public ReferenceException(string correlationId, string code, string message)
            : base(correlationId, code, message)
        { }

#if !CORE_NET
        protected ReferenceException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

    }
}
