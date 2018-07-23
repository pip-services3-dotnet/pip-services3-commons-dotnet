using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PipServices.Commons.Run;
using PipServices.Commons.Validate;

namespace PipServices.Commons.Commands
{
    /// <summary>
    /// Interceptor wrapper to turn it into a stackable command.
    /// </summary>
    public class InterceptedCommand : ICommand
    {
        private readonly ICommandIntercepter _intercepter;
        private readonly ICommand _next;

        /// <summary>
        /// Creates an instance of intercepted command.
        /// </summary>
        /// <param name="intercepter">Intercepter reference.</param>
        /// <param name="next">Next intercepter or command in the chain.</param>
        public InterceptedCommand(ICommandIntercepter intercepter, ICommand next)
        {
            _intercepter = intercepter;
            _next = next;
        }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name
        {
            get { return _intercepter.GetName(_next); }
        }

        /// <summary>
        /// Executes the command given specific arguments as input.
        /// </summary>
        /// <param name="correlationId">Unique correlation/transaction id.</param>
        /// <param name="args">Command arguments.</param>
        /// <returns>Execution result.</returns>
        public Task<object> ExecuteAsync(string correlationId, Parameters args)
        {
            return _intercepter.ExecuteAsync(correlationId, _next, args);
        }

        /// <summary>
        /// Performs validation of the command arguments.
        /// </summary>
        /// <param name="args">Command arguments.</param>
        /// <returns>A list of errors or an empty list if validation was successful.</returns>
        public IList<ValidationResult> Validate(Parameters args)
        {
            return _intercepter.Validate(_next, args);
        }
    }
}