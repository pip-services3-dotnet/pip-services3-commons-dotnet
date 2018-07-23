using PipServices.Commons.Errors;
using PipServices.Commons.Run;
using PipServices.Commons.Validate;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipServices.Commons.Commands
{
    public delegate Task<object> ExecutableDelegate(string correlationId, Parameters args);

    /// <summary>
    /// Represents a command that implements a command pattern.
    /// </summary>
    public class Command : ICommand
    {
        private readonly Schema _schema;
        private readonly ExecutableDelegate _function;

        /// <summary>
        /// Command constructor
        /// </summary>
        /// <param name="name">Command name.</param>
        /// <param name="schema">Command schema.</param>
        /// <param name="function">Executable function.</param>
        public Command(string name, Schema schema, ExecutableDelegate function)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            _schema = schema;
            _function = function;
        }

        public string Name { get; }

        /// <summary>
        /// Executes the command given specific arguments as input.
        /// </summary>
        /// <param name="correlationId">Unique correlation/transaction id.</param>
        /// <param name="args">Command arguments.</param>
        /// <returns>Execution result.</returns>
        public async Task<object> ExecuteAsync(string correlationId, Parameters args)
        {
            if (_schema != null)
            {
                _schema.ValidateAndThrowException(correlationId, args);
            }

            try
            {
                return await _function.Invoke(correlationId, args);
            }
            catch (Exception ex)
            {
                throw new InvocationException(
                    correlationId, 
                    "EXEC_FAILED", 
                    "Execution " + Name + " failed: " + ex
                )
                .WithDetails("command", Name)
                .Wrap(ex);
            }
        }

        /// <summary>
        /// Performs validation of the command arguments.
        /// </summary>
        /// <param name="args">Command arguments.</param>
        /// <returns>A list with validation results</returns>
        public IList<ValidationResult> Validate(Parameters args)
        {
            if (_schema != null)
            {
                return _schema.Validate(args);
            }

            return new List<ValidationResult>();
        }
    }
}