using System;
using System.Collections.Generic;
using PipServices.Commons.Run;
using PipServices.Commons.Errors;
using System.Threading.Tasks;

namespace PipServices.Commons.Commands
{
    /// <summary>
    /// Events to receit notifications on command execution results and failures.
    /// </summary>
    public class Event : IEvent
    {
        /// <summary>
        /// Creates an instance of Event.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        public Event(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            Name = name;
        }

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets listeners that receive notifications for this event.
        /// </summary>
        public List<IEventListener> Listeners { get; } = new List<IEventListener>();

        /// <summary>
        /// Adds a listener to receive notifications.
        /// </summary>
        /// <param name="listener">The listener to be added.</param>
        public void AddListener(IEventListener listener)
        {
            Listeners.Add(listener);
        }

        /// <summary>
        /// Removes a listener from event notifications.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void RemoveListener(IEventListener listener)
        {
            Listeners.Remove(listener);
        }

        /// <summary>
        /// Notifies all listeners about the event.
        /// </summary>
        /// <param name="correlationId">Unique correlation/transaction id.</param>
        /// <param name="args">The event arguments/value.</param>
        public async Task NotifyAsync(string correlationId, Parameters args)
        {
            foreach (var listener in Listeners)
            {
                try
                {
                    listener.OnEvent(correlationId, this, args);
                }
                catch (Exception ex)
                {
                    throw new InvocationException(
                        correlationId,
                        "EXEC_FAILED",
                        "Raising event " + Name + " failed: " + ex
                    )
                    .WithDetails("event", Name)
                    .Wrap(ex);
                }
            }

            await Task.Delay(0);
        }
    }
}