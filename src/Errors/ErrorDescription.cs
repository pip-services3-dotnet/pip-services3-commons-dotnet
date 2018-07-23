using System.Runtime.Serialization;
using PipServices.Commons.Data;

namespace PipServices.Commons.Errors
{
    [DataContract]
    public class ErrorDescription
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "details")]
        public StringValueMap Details { get; set; }

        [DataMember(Name = "correlation_id")]
        public string CorrelationId { get; set; }

        [DataMember(Name = "cause")]
        public string Cause { get; set; }

        [DataMember(Name = "stack_trace")]
        public string StackTrace { get; set; }
    }
}