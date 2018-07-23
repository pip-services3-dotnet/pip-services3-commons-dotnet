using PipServices.Commons.Run;

namespace PipServices.Commons.Commands
{
    /// <summary>
    /// Listener for command events.
    /// </summary>
    public interface IEventListener
    {
        /// <summary>
        /// Notifies that an event occurred.
        /// </summary>
        /// <param name="e">Event reference.</param>
        /// <param name="correlationId">Unique correlation/transaction id.</param>
        /// <param name="value">Event arguments/value.</param>
        void OnEvent(string correlationId, IEvent e, Parameters value);
    }
}