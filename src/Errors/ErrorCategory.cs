﻿namespace PipServices3.Commons.Errors
{
    /// <summary>
    /// Defines standard error categories to application exceptions supported by PipServices toolkit.
    /// </summary>
    public class ErrorCategory
    {
        /// <summary>
        /// Unknown or unexpected errors.
        /// </summary>
        public const string Unknown = "Unknown";

        /// <summary>
        /// Internal errors caused by programming mistakes.
        /// </summary>
        public const string Internal = "Internal";

        /// <summary>
        /// Errors related to mistakes in user-defined configuration.
        /// </summary>
        public const string Misconfiguration = "Misconfiguration";

        /// <summary>
        /// Errors related to operations called in wrong component state. 
        /// For instance, business calls when component is not ready.
        /// </summary>
        public const string InvalidState = "InvalidState";

        /// <summary>
        /// Errors caused by remote calls timeouted and not returning results.
        /// It allows to clearly separate communication related problems
        /// from other application errors.
        /// </summary>
        public const string NoResponse = "NoResponse";

        /// <summary>
        /// Errors returned by remote services or network during call attempts.
        /// </summary>
        public const string FailedInvocation = "FailedInvocation";

        /// <summary>
        /// Errors in read/write file operations.
        /// </summary>
        public const string NoFileAccess = "NoFileAccess";

        /// <summary>
        /// Errors due to improper user requests, like missing or wrong parameters.
        /// </summary>
        public const string BadRequest = "BadRequest";

        /// <summary>
        /// Access errors caused by missing user identity or security permissions.
        /// </summary>
        public const string Unauthorized = "Unauthorized";

        /// <summary>
        /// Error caused by attempt to access missing object.
        /// </summary>
        public const string NotFound = "NotFound";

        /// <summary>
        /// Errors raised by conflict in object versions posted by user and stored on server.
        /// </summary>
        public const string Conflict = "Conflict";

        /// <summary>
        /// Errors caused by calls to unsupported or not yet implemented functionality.
        /// </summary>
        public const string Unsupported = "Unsupported";

        /// <summary>
        /// Errors caused by too many requests.
        /// </summary>
        public const string TooManyRequests = "TooManyRequests";
    }
}
