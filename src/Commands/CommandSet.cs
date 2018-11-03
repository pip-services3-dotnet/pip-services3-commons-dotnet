using PipServices3.Commons.Data;
using PipServices3.Commons.Errors;
using PipServices3.Commons.Run;
using PipServices3.Commons.Validate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipServices3.Commons.Commands
{
    /// <summary>
    /// Contains a set of commands and events supported by a ICommandable object.
    /// The CommandSet supports command interceptors to extend and the command call chain.
    /// CommandSets can be used as alternative commandable interface to a business object.
    /// It can be used to auto generate multiple external services for the business object
    /// without writing much code.
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyDataCommandSet: CommandSet 
    /// {
    ///     private IMyDataController _controller;
    /// 
    ///     public MyDataCommandSet(IMyDataController controller)  // Any data controller interface
    ///     {
    ///         base();
    ///         this._controller = controller;
    ///         this.addCommand(this.MakeGetMyDataCommand()); }
    ///     
    ///         private ICommand MakeGetMyDataCommand() 
    ///         {
    ///             return new Command(
    ///             'get_mydata', 
    ///             null,
    ///             async(correlationId, args)=> {
    ///                 String param = args.getAsString('param');
    ///                 return this._controller.getMyData(correlationId, param);  });
    ///         }
    /// }
    /// </code>
    /// </example>
    /// See <see cref="ICommandable"/>, <see cref="Command"/>, <see cref="Event"/>
    public class CommandSet
    {
        private List<ICommand> _commands = new List<ICommand>();
        private List<IEvent> _events = new List<IEvent>();
        private readonly Dictionary<string, ICommand> _commandsByName = new Dictionary<string, ICommand>();
        private readonly Dictionary<string, IEvent> _eventsByName = new Dictionary<string, IEvent>();
        private readonly List<ICommandInterceptor> _intercepters = new List<ICommandInterceptor>();

        /// <summary>
        /// Creates an empty CommandSet object.
        /// </summary>
        public CommandSet() { }

        /// <summary>
        /// Gets all commands registered in this command set.
        /// </summary>
        /// <returns>a list of commands.</returns>
        /// See <see cref="ICommand"/>
        public List<ICommand> Commands
        {
            get { return _commands; }
        }

        /// <summary>
        /// Gets all events registred in this command set.
        /// </summary>
        /// <returns>a list of events.</returns>
        /// See <see cref="IEvent"/>
        private List<IEvent> Events
        {
            get { return _events; }
        }

        /// <summary>
        /// Searches for a command by its name.
        /// </summary>
        /// <param name="command">The command name.</param>
        /// <returns>A command with the given name.</returns>
        /// See <see cref="ICommand"/>
        public ICommand FindCommand(string command)
        {
            ICommand value;
            _commandsByName.TryGetValue(command, out value);
            return value;
        }

        /// <summary>
        /// Searches for an event by its name in this command set.
        /// </summary>
        /// <param name="ev">the name of the event to search for.</param>
        /// <returns>An event with the given name.</returns>
        /// See <see cref="IEvent"/>
        public IEvent FindEvent(string ev)
        {
            IEvent value;
            _eventsByName.TryGetValue(ev, out value);
            return value;
        }

        private void BuildCommandChain(ICommand command)
        {
            var next = command;

            for (var i = _intercepters.Count - 1; i >= 0; i--)
                next = new InterceptedCommand(_intercepters[i], next);

            _commandsByName[next.Name] = next;
        }

        private void RebuildAllCommandChains()
        {
            _commandsByName.Clear();

            foreach (var command in _commands)
                BuildCommandChain(command);
        }

        /// <summary>
        /// Adds a ICommand command to this command set.
        /// </summary>
        /// <param name="command">The command to add.</param>
        /// See <see cref="ICommand"/>
        public void AddCommand(ICommand command)
        {
            Commands.Add(command);
            BuildCommandChain(command);
        }

        /// <summary>
        /// Adds multiple ICommand commands to this command set.
        /// </summary>
        /// <param name="commands">The commands to add.</param>
        /// See <see cref="ICommand"/>
        public void AddCommands(IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
                AddCommand(command);
        }

        /// <summary>
        /// Adds an IEvent event to this command set.
        /// </summary>
        /// <param name="ev">The event to add.</param>
        /// See <see cref="IEvent"/>
        public void AddEvent(IEvent ev)
        {
            _events.Add(ev);
            _eventsByName[ev.Name] = ev;
        }

        /// <summary>
        /// Adds multiple IEvent events to this command set.
        /// </summary>
        /// <param name="events">The events to add.</param>
        /// See <see cref="IEvent"/>
        public void AddEvents(IEnumerable<IEvent> events)
        {
            foreach (var ev in events)
                AddEvent(ev);
        }

        /// <summary>
        /// Adds all of the commands and events from specified CommandSet command set
        /// into this one.
        /// </summary>
        /// <param name="commandSet">The commands set to add.</param>
        public void AddCommandSet(CommandSet commandSet)
        {
            foreach (var command in commandSet.Commands)
                AddCommand(command);

            foreach (var commandEvent in commandSet.Events)
                AddEvent(commandEvent);
        }


        /// <summary>
        /// Adds a ICommandInterceptor command interceptor to this command set.
        /// </summary>
        /// <param name="intercepter">The intercepter to add.</param>
        /// See <see cref="ICommandInterceptor"/>
        public void AddInterceptor(ICommandInterceptor intercepter)
        {
            _intercepters.Add(intercepter);
            RebuildAllCommandChains();
        }

        /// <summary>
        /// Executes a ICommand command specificed by its name.
        /// </summary>
        /// <param name="correlationId">Unique correlation/transaction id.</param>
        /// <param name="command">Command name.</param>
        /// <param name="args">Command arguments.</param>
        /// <exception cref="ValidationException"> when execution fails for validation reason.</exception>
        /// <returns>Execution result.</returns>
        /// See <see cref="ICommand"/>,
        /// <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-commons-dotnet/master/doc/api/class_pip_services_1_1_commons_1_1_run_1_1_parameters.html"/>Parameters</a>
        public Task<object> ExecuteAsync(string correlationId, string command, Parameters args)
        {
            var cref = FindCommand(command);
            if (cref == null)
            {
                throw new BadRequestException(
                    correlationId,
                    "CMD_NOT_FOUND",
                    "Request command does not exist"
                )
                .WithDetails("command", command);
            }

            if (correlationId == null)
                correlationId = IdGenerator.NextShort();

            var results = cref.Validate(args);
            ValidationException.ThrowExceptionIfNeeded(correlationId, results, false);

            return cref.ExecuteAsync(correlationId, args);
        }

        /// <summary>
        /// Validates Parameters args for command specified by its name using defined
        /// schema.If validation schema is not defined than the methods returns no
        /// errors.It returns validation error if the command is not found.
        /// </summary>
        /// <param name="command">the name of the command for which the 'args' must be validated.</param>
        /// <param name="args">the parameters (arguments) to validate.</param>
        /// <returns>a list of ValidationResults. If no command is found by the given
        /// name, then the returned array of ValidationResults will contain a
        /// single entry, whose type will be ValidationResultType.Error.</returns>
        /// See <see cref="Command"/>,
        /// <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-commons-dotnet/master/doc/api/class_pip_services_1_1_commons_1_1_run_1_1_parameters.html"/>Parameters</a>, 
        /// <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-commons-dotnet/master/doc/api/class_pip_services_1_1_commons_1_1_validate_1_1_validation_result.html"/>ValidationResult</a>
        public IList<ValidationResult> Validate(string command, Parameters args)
        {
            var cref = FindCommand(command);

            if (cref == null)
            {
                var results = new List<ValidationResult>
                {
                    new ValidationResult(null, ValidationResultType.Error,
                        "CMD_NOT_FOUND", "Requested command does not exist", null, null)
                };

                return results;
            }

            return cref.Validate(args);
        }

        /// <summary>
        /// Adds a IEventListener listener to receive notifications on fired events.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void AddListener(IEventListener listener)
        {
            foreach (var ev in Events)
            {
                ev.AddListener(listener);
            }
        }

        /// <summary>
        /// Removes previosly added IEventListener listener.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void RemoveListener(IEventListener listener)
        {
            foreach (var ev in _events)
            {
                ev.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Fires event specified by its name and notifies all registered IEventListener listeners
        /// </summary>
        /// <param name="correlationId">optional transaction id to trace calls across components.</param>
        /// <param name="ev">the name of the event that is to be fired.</param>
        /// <param name="value">the event arguments (parameters).</param>
        public async Task NotifyAsync(string correlationId, string ev, Parameters value)
        {
            var e = FindEvent(ev);

            if (e != null)
                await e.NotifyAsync(correlationId, value);
        }
    }
}