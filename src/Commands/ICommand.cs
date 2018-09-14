using System.Collections.Generic;
using PipServices.Commons.Run;
using PipServices.Commons.Validate;

namespace PipServices.Commons.Commands
{
    /// <summary>
    /// An interface for Commands, which are part of the Command design pattern. 
    /// Each command wraps a method or function and allows to call them in uniform and safe manner.
    /// </summary>
    /// See <see cref="Command"/>, <see cref="IExecutable"/>, <see cref="ICommandInterceptor"/>, <see cref="InterceptedCommand"/>
    public interface ICommand : IExecutable
    {
        /// <summary>
        /// Gets the command name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Validates command arguments before execution using defined schema.
        /// </summary>
        /// <param name="args">the parameters (arguments) to validate.</param>
        /// <returns>List of errors or empty list if validation was successful.</returns>
        /// See <see cref="Parameters"/>, <see cref="ValidationResult"/>
        IList<ValidationResult> Validate(Parameters args);
    }
}