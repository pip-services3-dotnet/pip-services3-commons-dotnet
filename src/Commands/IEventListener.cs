using PipServices3.Commons.Run;

namespace PipServices3.Commons.Commands
{
    /// <summary>
    /// An interface for listener objects that receive notifications on fired events.
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyListener: IEventListener {
    ///     private Task onEvent(String correlationId, IEvent event, Parameters args)  {
    ///         Console.WriteLine("Fired event " + event.getName());
    ///     }}
    ///     
    /// Event event = new Event("myevent");
    /// event.addListener(new MyListener()); 
    /// event.notify("123", Parameters.fromTuples("param1", "ABC")); 
    /// // Console output: Fired event myevent
    /// </code>
    /// </example>
    /// See <see cref="IEvent"/>, <see cref="Event"/>
public interface IEventListener
    {
        /// <summary>
        /// A method called when events this listener is subscrubed to are fired.
        /// </summary>
        /// <param name="e">a fired event</param>
        /// <param name="correlationId">optional transaction id to trace calls across components.</param>
        /// <param name="value">Event arguments/value.</param>
        void OnEvent(string correlationId, IEvent e, Parameters value);
    }
}