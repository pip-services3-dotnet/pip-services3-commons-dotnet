using System;
using System.Collections.Generic;
using PipServices3.Commons.Run;
using PipServices3.Commons.Errors;
using System.Threading.Tasks;

namespace PipServices3.Commons.Commands
{
    /// <summary>
    /// Concrete implementation of IEvent interface.
    /// It allows to send asynchronous notifications to multiple subscribed listeners.
    /// </summary>
    /// <example>
    /// <code>
    /// var event = new Event("my_event");
    /// event.addListener(myListener);
    /// 
    /// event.notify("123", Parameters.fromTuples(
    /// "param1", "ABC",   
    /// "param2", 123 ));
    /// </code>
    /// </example>
    /// See <see cref="IEvent"/>, <see cref="IEventListener"/>
    public class Event : IEvent
    {
        /// <summary>
        /// Creates a new event and assigns its name.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <exception cref="ArgumentNullException">an Error if the name is null.</exception>
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
        /// Gets all listeners registered in this event.
        /// </summary>
        public List<IEventListener> Listeners { get; } = new List<IEventListener>();

        /// <summary>
        /// Adds a listener to receive notifications when this event is fired.
        /// </summary>
        /// <param name="listener">Tthe listener reference to add.</param>
        public void AddListener(IEventListener listener)
        {
            Listeners.Add(listener);
        }

        /// <summary>
        ///  Removes a listener, so that it no longer receives notifications for this event.
        /// </summary>
        /// <param name="listener">the listener reference to remove.</param>
        public void RemoveListener(IEventListener listener)
        {
            Listeners.Remove(listener);
        }

        /// <summary>
        /// Fires this event and notifies all registered listeners.
        /// </summary>
        /// <param name="correlationId">Unique correlation/transaction id.</param>
        /// <param name="args">The event arguments/value.</param>
        /// <exception cref="InvocationException">if the event fails to be raised.</exception>
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