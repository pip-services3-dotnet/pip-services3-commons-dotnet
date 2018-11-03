using System.Runtime.Serialization;
using PipServices3.Commons.Data;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Serializeable error description. It is use to pass information about errors
    /// between microservices implemented in different languages. On the receiving side
    /// ErrorDescription is used to recreate exception object close to its original type
    /// without missing additional details.
    /// </summary>
    /// See <see cref="ApplicationException"/>, <see cref="ApplicationExceptionFactory"/>
    [DataContract]
    public class ErrorDescription
    {
        /** Data type of the original error */
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /** Standard error category */
        [DataMember(Name = "category")]
        public string Category { get; set; }

        /** HTTP status code associated with this error type */
        [DataMember(Name = "status")]
        public int Status { get; set; }

        /** A unique error code */
        [DataMember(Name = "code")]
        public string Code { get; set; }

        /** A human-readable error description (usually written in English) */
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /** A map with additional details that can be used to restore error description in other languages */
        [DataMember(Name = "details")]
        public StringValueMap Details { get; set; }

        /** A unique transaction id to trace execution throug call chain */
        [DataMember(Name = "correlation_id")]
        public string CorrelationId { get; set; }

        /** Original error wrapped by this exception */
        [DataMember(Name = "cause")]
        public string Cause { get; set; }

        /** Stack trace of the exception */
        [DataMember(Name = "stack_trace")]
        public string StackTrace { get; set; }
    }
}