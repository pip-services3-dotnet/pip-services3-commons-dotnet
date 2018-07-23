using System.Collections.Generic;
using PipServices.Commons.Run;

namespace PipServices.Commons.Commands
{
    /// <summary>
    /// Interface for command events.
    /// </summary>
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
        /// Adds a listener to receive notifications.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        void AddListener(IEventListener listener);

        /// <summary>
        /// Removes a listener from event notifications.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        void RemoveListener(IEventListener listener);
    }
}