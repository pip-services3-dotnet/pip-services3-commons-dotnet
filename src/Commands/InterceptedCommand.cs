using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PipServices3.Commons.Run;
using PipServices3.Commons.Validate;

namespace PipServices3.Commons.Commands
{
    /// <summary>
    /// Implements a ICommand command wrapped by an interceptor.
    /// It allows to build command call chains.The interceptor can alter execution
    /// and delegate calls to a next command, which can be intercepted or concrete.
    /// </summary>
    /// <example>
    /// <code>
    /// public class CommandLogger: ICommandInterceptor
    /// {
    ///     public String GetName(ICommand command) 
    ///     {
    ///         return command.GetName();
    ///     }
    ///     
    ///     public Task<object> ExecuteAsync(string correlationId, ICommand command, Parameters args) 
    ///     {
    ///         Console.WriteLine("Executed command " + command.getName());
    ///         return command.ExecuteAsync(correlationId, args); 
    ///     }
    ///     
    ///     private IList<ValidationResult> validate(ICommand command, Parameters args) 
    ///     {
    ///         return command.validate(args);
    ///     }
    /// }
    /// var logger = new CommandLogger();
    /// var loggedCommand = new InterceptedCommand(logger, command);
    /// // Each called command will output: Executed command <command name>
    /// </code>
    /// </example>
    /// See <see cref="ICommand"/>, <see cref="ICommandInterceptor"/>
    public class InterceptedCommand : ICommand
    {
        private readonly ICommandInterceptor _interceptor;
        private readonly ICommand _next;

        /// <summary>
        /// Creates a new InterceptedCommand, which serves as a link in an execution
        /// chain.Contains information about the interceptor that is being used and the
        /// next command in the chain.
        /// </summary>
        /// <param name="interceptor">the interceptor that is intercepting the command.</param>
        /// <param name="next">Next intercepter or command in the chain.</param>
        public InterceptedCommand(ICommandInterceptor interceptor, ICommand next)
        {
            _interceptor = interceptor;
            _next = next;
        }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        /// <returns>the name of the command that is being intercepted.</returns>
        public string Name
        {
            get { return _interceptor.GetName(_next); }
        }

        /// <summary>
        /// Gets the command schema.
        /// </summary>
        public Schema Schema => _next?.Schema;

        /// <summary>
        /// Executes the next command in the execution chain using the given Parameters parameters(arguments).
        /// </summary>
        /// <param name="correlationId">unique transaction id to trace calls across components.</param>
        /// <param name="args">the parameters (arguments) to pass to the command for execution.</param>
        /// <returns>Execution result.</returns>
        /// See <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-commons-dotnet/master/doc/api/class_pip_services_1_1_commons_1_1_run_1_1_parameters.html"/>Parameters</a>
        public Task<object> ExecuteAsync(string correlationId, Parameters args)
        {
            return _interceptor.ExecuteAsync(correlationId, _next, args);
        }

        /// <summary>
        /// Validates the Parameters args that are to be passed to the command that is next in the execution chain.
        /// </summary>
        /// <param name="args">the parameters (arguments) to validate for the next command.</param>
        /// <returns>A list of errors or an empty list if validation was successful.</returns>
        /// See <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-commons-dotnet/master/doc/api/class_pip_services_1_1_commons_1_1_run_1_1_parameters.html"/>Parameters</a>, 
        /// <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-commons-dotnet/master/doc/api/class_pip_services_1_1_commons_1_1_validate_1_1_validation_result.html"/>ValidationResult</a>
        public IList<ValidationResult> Validate(Parameters args)
        {
            return _interceptor.Validate(_next, args);
        }
    }
}