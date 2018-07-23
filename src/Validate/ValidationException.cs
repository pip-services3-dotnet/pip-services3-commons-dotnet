using PipServices.Commons.Errors;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PipServices.Commons.Validate
{
#if CORE_NET
    [DataContract]
#else
    [Serializable]
#endif
    public class ValidationException : BadRequestException
    {
        private static long SerialVersionUid { get; } = -1459801864235223845L;

        public ValidationException(string correlationId, IList<ValidationResult> results) :
            this(correlationId, ComposeMessage(results))
        {
            WithDetails("results", results);
        }

        public ValidationException(string correlationId, string message) :
            base(correlationId, "INVALID_DATA", message)
        {
        }

#if !CORE_NET
        protected ValidationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
#endif

        public static string ComposeMessage(IList<ValidationResult> results)
        {
            var builder = new StringBuilder();
            builder.Append("Validation failed");

            if (results != null && results.Count > 0)
            {
                var first = true;
                foreach (var result in results)
                {
                    if (result.Type == ValidationResultType.Information)
                        continue;

                    builder.Append(!first ? ": " : ", ");
                    builder.Append(result.Message);
                    first = false;
                }
            }

            return builder.ToString();
        }


        public static void ThrowExceptionIfNeeded(string correlationId, IList<ValidationResult> results, bool strict)
        {
            var hasErrors = false;
            foreach (var result in results)
            {
                if (result.Type == ValidationResultType.Error)
                    hasErrors = true;

                if (strict && result.Type == ValidationResultType.Warning)
                    hasErrors = true;
            }

            if (hasErrors)
                throw new ValidationException(correlationId, results);
        }
    }
}