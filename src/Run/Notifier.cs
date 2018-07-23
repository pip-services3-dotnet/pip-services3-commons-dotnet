using System.Collections;
using System.Threading.Tasks;

namespace PipServices.Commons.Run
{
    /// <summary>
    /// Helper class that triggers notification for components
    /// </summary>
    public class Notifier
    {
        /// <summary>
        /// Triggers notification for component that implement INotifiable interface.
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <param name="component">a omponents to be notified</param>
        /// <param name="args">a set of parameters to pass to notified components</param>
        public static async Task NotifyOneAsync(string correlationId, object component, Parameters args)
        {
                var notifiable = component as INotifiable;
                if (notifiable != null)
                    await notifiable.NotifyAsync(correlationId, args);
        }

        /// <summary>
        /// Triggers notification for components that implement INotifiable and IParamParam interfaces
        /// and passes to IParamNotifiable them set of parameters.
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <param name="components">a list of components to be notified</param>
        /// <param name="args">a set of parameters to pass to notified components</param>
        public static async Task NotifyAsync(string correlationId, IEnumerable components, Parameters args)
        {
            if (components == null) return;

            foreach (var component in components)
                await NotifyOneAsync(correlationId, component, args);
        }
    }
}