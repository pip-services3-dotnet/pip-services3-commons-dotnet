using System;
using PipServices3.Commons.Data;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PipServices3.Commons.Reflect;

namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Defines a base class to defive various application exceptions.
    /// 
    /// Most languages have own definition of base exception(error) types.
    /// However, this class is implemented symmetrically in all languages
    /// supported by PipServices toolkit.It allows to create portable implementations
    /// and support proper error propagation in microservices calls.
    /// 
    /// Error propagation means that when microservice implemented in one language
    /// calls microservice(s) implemented in a different language(s), errors are returned
    /// throught the entire call chain and restored in their original (or close) type.
    /// 
    /// Since number of potential exception types is endless, PipServices toolkit
    /// supports only 12 standard categories of exceptions defined in ErrorCategory.
    /// This ApplicationException class acts as a basis for
    /// all other 12 standard exception types.
    /// 
    /// Most exceptions have just free-form message that describes occured error.
    /// That may not be sufficient to create meaninful error descriptions.
    /// The ApplicationException class proposes an extended error definition
    /// that has more standard fields:
    /// 
    /// - message: is a humand readable error description
    /// - category: one of 12 standard error categories of errors
    /// - status: numeric HTTP status code for REST invocations
    /// - code: a unique error code, usually defined as "MY_ERROR_CODE"
    /// - correlation_id: a unique transaction id to trace execution through a call chain
    /// - details: map with error parameters that can help to recreate meaningful error description in other languages
    /// - stack_trace: a stack trace
    /// - cause: original error that is wrapped by this exception
    /// 
    /// ApplicationException class is not serializable.To pass errors through the wire
    /// it is converted into ErrorDescription object and restored on receiving end into
    /// identical exception type.
    /// </summary>
    /// See <see cref="ErrorCategory"/>, <see cref="ErrorDescription"/>
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class ApplicationException : Exception
    {
        private string _stackTrace;

        /// <summary>
        /// Creates a new instance of application exception.
        /// </summary>
        public ApplicationException() :
            this(null, null, null, null)
        { }

        /// <summary>
        /// Creates a new instance of application exception with string message.
        /// </summary>
        /// <param name="message">a human-readable description of the error.</param>
        [JsonConstructor]
        protected ApplicationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of application exception and assigns its values.
        /// </summary>
        /// <param name="category">(optional) a standard error category. Default: Unknown</param>
        /// <param name="correlationId">(optional) a unique transaction id to trace execution
        /// through call chain.</param>
        /// <param name="code">(optional) a unique error code. Default: "UNKNOWN"</param>
        /// <param name="message">(optional) a human-readable description of the error.</param>
        public ApplicationException(string category = null, string correlationId = null, string code = null, string message = null)
            : base(message ?? "Unknown error")
        {
            Code = "UNKNOWN";
            Status = 500;

            Category = category ?? ErrorCategory.Unknown;
            CorrelationId = correlationId;
            Code = code;
        }

#if !CORE_NET
        protected ApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Category = info.GetString("category");
            CorrelationId = info.GetString("correlation_id");
            Cause = info.GetString("cause");
            Code = info.GetString("code");
            Status = info.GetInt32("status");
            _stackTrace = info.GetString("stack_trace");
            Details = StringValueMap.FromString(info.GetString("details"));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("category", Category);
            info.AddValue("correlation_id", CorrelationId);
            info.AddValue("cause", Cause);
            info.AddValue("code", Code);
            info.AddValue("status", Status);
            info.AddValue("stack_trace", StackTrace);
            info.AddValue("details", Details != null ? Details.ToString() : null);
        }
#endif
        /** Standard error category */
        [JsonProperty("category")]
        public string Category { get; set; }

        /** A unique transaction id to trace execution throug call chain */
        [JsonProperty("correlation_id")]
        public string CorrelationId { get; set; }

        /** Original error wrapped by this exception */
        [JsonProperty("cause")]
        public string Cause { get; set; }

        /** A unique error code */
        [JsonProperty("code")]
        public string Code { get; set; }

        /** HTTP status code associated with this error type */
        [JsonProperty("status")]
        public int Status { get; set; }

        /** A map with additional details that can be used to restore error description in other languages */
        [JsonProperty("details")]
        public StringValueMap Details { get; set; }

        /** A human-readable error description (usually written in English) */
        [JsonProperty("message")]
        public string BaseMessage => Message;

        /** Stack trace of the exception */
        [JsonProperty("stack_trace")]
        public new string StackTrace
        {
            get { return _stackTrace ?? base.StackTrace; }
            set { _stackTrace = value; }
        }

        /// <summary>
        /// Sets a unique error code.
        /// 
        /// This method returns reference to this exception to implement Builder pattern
        /// to chain additional calls.
        /// </summary>
        /// <param name="code">a unique error code</param>
        /// <returns>this exception object</returns>
        public ApplicationException WithCode(string code)
        {
            Code = code ?? "UNKNOWN";
            return this;
        }

        /// <summary>
        /// Sets a correlation id which can be used to trace this error through a call chain.
        /// 
        /// This method returns reference to this exception to implement Builder pattern
        /// to chain additional calls.
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace error through call chain</param>
        /// <returns>this exception object</returns>
        public ApplicationException WithCorrelationId(string correlationId)
        {
            CorrelationId = correlationId;
            return this;
        }

        /// <summary>
        /// Sets a original error wrapped by this exception.
        /// 
        /// This method returns reference to this exception to implement Builder pattern
        /// to chain additional calls.
        /// </summary>
        /// <param name="cause">original error object</param>
        /// <returns>this exception object</returns>
        public ApplicationException WithCause(Exception cause)
        {
            Cause = cause?.Message;
            return this;
        }

        /// <summary>
        /// Sets a HTTP status code which shall be returned by REST calls.
        /// 
        /// This method returns reference to this exception 
        /// to implement Builder pattern to chain additional calls.
        /// </summary>
        /// <param name="status">an HTTP error code.</param>
        /// <returns>this exception object</returns>
        public ApplicationException WithStatus(int status)
        {
            Status = status;
            return this;
        }

        /// <summary>
        /// Sets a parameter for additional error details. This details can be used to
        /// restore error description in other languages.
        /// 
        /// This method returns reference to this exception to implement Builder pattern
        /// to chain additional calls.
        /// </summary>
        /// <param name="key">a details parameter key</param>
        /// <param name="value">a details parameter name</param>
        /// <returns>this exception object</returns>
        public ApplicationException WithDetails(string key, object value)
        {
            Details = Details ?? new StringValueMap();
            Details.SetAsObject(key, value);
            return this;
        }

        /// <summary>
        /// Sets a stack trace for this error.
        /// This method returns reference to this exception to implement Builder pattern
        /// to chain additional calls.
        /// </summary>
        /// <param name="stackTrace">a stack trace where this error occured</param>
        /// <returns>this exception object</returns>
        public ApplicationException WithStackTrace(string stackTrace)
        {
            _stackTrace = stackTrace;
            return this;
        }

        /// <summary>
        /// Wraps another exception into specified application exception object.
        /// If original exception is of ApplicationException type it is returned without
        /// changes.Otherwise the original error is set as a cause to specified ApplicationException object.
        /// </summary>
        /// <param name="cause">an original error object</param>
        /// <returns>an original or newly created ApplicationException</returns>
        public ApplicationException Wrap(Exception cause)
        {
            if (cause is ApplicationException)
                return (ApplicationException)cause;

            WithCause(cause);
            return this;
        }

        /// <summary>
        /// Wraps another exception into specified application exception object.
        /// If original exception is of ApplicationException type it is returned without
        /// changes.Otherwise the original error is set as a cause to specified ApplicationException object.
        /// </summary>
        /// <param name="error">an ApplicationException object to wrap the cause</param>
        /// <param name="cause">an original error object</param>
        /// <returns>an original or newly created ApplicationException</returns>
        public static ApplicationException WrapException(ApplicationException error, Exception cause)
        {
            if (cause is ApplicationException)
                return (ApplicationException)cause;

            error.WithCause(cause);
            return error;
        }
    }
}