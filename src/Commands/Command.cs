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
    /// Concrete implementation of ICommand interface. Command allows to call a method
    /// or function using Command pattern.
    /// </summary>
    /// <example>
    /// <code>
    /// var command = new Command("add", null, async(args) => {
    /// var param1 = args.getAsFloat("param1");
    /// var param2 = args.getAsFloat("param2");
    /// return param1 + param2; });
    /// var result = command.ExecuteAsync("123", Parameters.fromTuples(
    /// "param1", 2,
    /// "param2", 2 ));
    /// Console.WriteLine(result.ToString()); 
    /// // Console output: 4
    /// </code>
    /// </example>
    /// See <see cref="ICommand"/>, <see cref="CommandSet"/>
    public class Command : ICommand
    {
        private readonly Schema _schema;
        private readonly ExecutableDelegate _function;

        /// <summary>
        /// Creates a new command object and assigns it's parameters.
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
        /// Executes the command. Before execution is validates Parameters args using
        /// the defined schema.The command execution intercepts exceptions raised
        /// by the called function and calls them as an error.
        /// </summary>
        /// <param name="correlationId">Unique correlation/transaction id.</param>
        /// <param name="args">Command arguments.</param>
        /// <returns>Execution result.</returns>
        /// See <see cref="Parameters"/>
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
        /// Validates the command Parameters args before execution using the defined schema.
        /// </summary>
        /// <param name="args">Command arguments.</param>
        /// <returns>a list of ValidationResults or an empty array (if no schema is set).</returns>
        /// See <see cref="Parameters"/>, <see cref="ValidationResult"/>
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