using PipServices3.Commons.Errors;
using System;
using System.Runtime.Serialization;

namespace PipServices3.Commons.Refer
{
    /// <summary>
    /// Error when required component dependency cannot be found.
    /// </summary>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class ReferenceException : InternalException
    {
        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        public ReferenceException()
            : this(null, null)
        {
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="locator">the locator to find reference to dependent component.</param>
        public ReferenceException(object locator)
            : base(null, "REF_ERROR", "Failed to obtain reference to " + locator)
        {
            WithDetails("locator", locator);
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="correlationId">(optional) a unique transaction id to trace execution
        /// through call chain.</param>
        /// <param name="locator">the locator to find reference to dependent component.</param>
        public ReferenceException(string correlationId, object locator)
            : base(correlationId, "REF_ERROR", "Failed to obtain reference to " + locator)
        {
            WithDetails("locator", locator);
        }

        /// <summary>
        /// Creates an error instance and assigns its values.
        /// </summary>
        /// <param name="correlationId">(optional) a unique transaction id to trace execution
        /// through call chain.</param>
        /// <param name="message">(optional) a human-readable description of the error.</param>
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
