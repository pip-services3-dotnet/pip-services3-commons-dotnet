using System.Collections.Generic;
using PipServices3.Commons.Run;

namespace PipServices3.Commons.Commands
{
    /// <summary>
    /// An interface for Events, which are part of the Command design pattern. 
    /// Events allows to send asynchronous notifications to multiple subscribed listeners.
    /// </summary>
    /// See <see cref="IEventListener"/>
    public interface IEvent : INotifiable
    {
        /// <summary>
        /// Gets the event name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the listeners that receive notifications for this event.
        /// </summary>
        List<IEventListener> Listeners { get; }

        /// <summary>
        /// Adds a listener to receive notifications for this event.
        /// </summary>
        /// <param name="listener">the listener reference to add.</param>
        void AddListener(IEventListener listener);

        /// <summary>
        /// Removes a listener, so that it no longer receives notifications for this event.
        /// </summary>
        /// <param name="listener">the listener reference to remove.</param>
        void RemoveListener(IEventListener listener);
    }
}