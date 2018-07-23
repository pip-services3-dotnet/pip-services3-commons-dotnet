using PipServices.Commons.Data;
using PipServices.Commons.Errors;
using PipServices.Commons.Run;
using PipServices.Commons.Validate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipServices.Commons.Commands
{
    /// <summary>
    /// Handles command registration and execution.
    /// Enables interceptors to control or modify command behavior.
    /// </summary>
    public class CommandSet
    {
        private List<ICommand> _commands = new List<ICommand>();
        private List<IEvent> _events = new List<IEvent>();
        private readonly Dictionary<string, ICommand> _commandsByName = new Dictionary<string, ICommand>();
        private readonly Dictionary<string, IEvent> _eventsByName = new Dictionary<string, IEvent>();
        private readonly List<ICommandIntercepter> _intercepters = new List<ICommandIntercepter>();

        public CommandSet() { }

        /// <summary>
        /// Gets all supported commands.
        /// </summary>
        public List<ICommand> Commands
        {
            get { return _commands; }
        }

        /// <summary>
        /// Gets all supported events.
        /// </summary>
        private List<IEvent> Events
        {
            get { return _events; }
        }

        /// <summary>
        /// Finds a specific command by its name.
        /// </summary>
        /// <param name="command">The command name.</param>
        /// <returns>A command with the given name.</returns>
        public ICommand FindCommand(string command)
        {
            ICommand value;
            _commandsByName.TryGetValue(command, out value);
            return value;
        }

        /// <summary>
        /// Finds a specific event by its name.
        /// </summary>
        /// <param name="ev">The event name.</param>
        /// <returns>An event with the given name.</returns>
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
        /// Adds a command to the command set.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public void AddCommand(ICommand command)
        {
            Commands.Add(command);
            BuildCommandChain(command);
        }

        /// <summary>
        /// Adds commands to the command set.
        /// </summary>
        /// <param name="commands">The commands to add.</param>
        public void AddCommands(IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
                AddCommand(command);
        }

        /// <summary>
        /// Adds an event to the command set.
        /// </summary>
        /// <param name="ev">The event to add.</param>
        public void AddEvent(IEvent ev)
        {
            _events.Add(ev);
            _eventsByName[ev.Name] = ev;
        }

        /// <summary>
        /// Adds events to the command set.
        /// </summary>
        /// <param name="events">The events to add.</param>
        public void AddEvents(IEnumerable<IEvent> events)
        {
            foreach (var ev in events)
                AddEvent(ev);
        }

        /// <summary>
        /// Adds command from another command set to this set.
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
        /// Adds an intercepter to the command set.
        /// </summary>
        /// <param name="intercepter">The intercepter to add.</param>
        public void AddInterceptor(ICommandIntercepter intercepter)
        {
            _intercepters.Add(intercepter);
            RebuildAllCommandChains();
        }

        /// <summary>
        /// Executes a command by its name with specified arguments.
        /// </summary>
        /// <param name="correlationId">Unique correlation/transaction id.</param>
        /// <param name="command">Command name.</param>
        /// <param name="args">Command arguments.</param>
        /// <returns>Execution result.</returns>
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
        /// Validates command arguments.
        /// </summary>
        /// <param name="command">Command name.</param>
        /// <param name="args">Command arguments.</param>
        /// <returns>A list of validation errors or an empty list if the arguments are valid.</returns>
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
        /// Adds listener to all events.
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
        /// Removes a listener from all events.
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
        /// Notifies all listeners about the event.
        /// </summary>
        /// <param name="ev">Event name.</param>
        /// <param name="correlationId">Correlation/transaction id.</param>
        /// <param name="value">Event arguments/value.</param>
        public async Task NotifyAsync(string correlationId, string ev, Parameters value)
        {
            var e = FindEvent(ev);

            if (e != null)
                await e.NotifyAsync(correlationId, value);
        }
    }
}