using System.Threading.Tasks;

namespace PipServices3.Commons.Run
{
    /// <summary>
    /// Interface for components that can be asynchronously notified.
    /// The notification may include optional argument that describe the occured event.
    /// </summary>
    /// <example>
    /// <code>
    /// class MyComponent: INotifable 
    /// {
    ///     ...
    ///     public void Notify(string correlationId, Parameters args)
    ///     {
    ///         Console.WriteLine("Occured event " + args.GetAsString("event"));
    ///     }
    /// }
    /// 
    /// var myComponent = new MyComponent();
    /// myComponent.Notify("123", Parameters.FromTuples("event", "Test Event"));
    /// </code>
    /// </example>
    /// See <see cref="Notifier"/>, <see cref="IExecutable"/>
    public interface INotifiable
    {
        /// <summary>
        /// Notifies the component about occured event.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="args">notification arguments.</param>
        Task NotifyAsync(string correlationId, Parameters args);
    }
}
