namespace PipServices3.Commons.Config
{
    /// <summary>
    /// An interface to set configuration parameters to an object.
    /// 
    /// It can be added to any existing class by implementing a single <c>Configure()</c> method.
    /// 
    /// If you need to emphasis the fact that <c>Configure()</c> method can be called multiple times
    /// to change object configuration in runtime, use IReconfigurable interface instead.
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyClass: IConfigurable 
    /// {
    ///     private var _myParam = "default value";
    ///     public Task configure(ConfigParams config)
    ///     {
    ///         this._myParam = config.getAsStringWithDefault("options.param", myParam);
    ///         ...
    ///     }
    /// }
    /// </code>
    /// </example>
    /// See <see cref="ConfigParams"/>
    public interface IConfigurable
    {
        /// <summary>
        /// Configures object by passing configuration parameters.
        /// </summary>
        /// <param name="config">configuration parameters to be set.</param>
        void Configure(ConfigParams config);
    }
}