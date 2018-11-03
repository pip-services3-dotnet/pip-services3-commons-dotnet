namespace PipServices3.Commons.Commands
{
    /// <summary>
    /// An interface for commandable objects, which are part of the command design pattern.
    /// The commandable object exposes its functonality as commands and events grouped into a CommandSet.
    /// This interface is typically implemented by controllers and is used to auto generate external interfaces.
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyDataController: ICommandable, IMyDataController
    /// {
    ///     private MyDataCommandSet _commandSet;
    ///     
    ///     public CommandSet getCommandSet() 
    ///     {
    ///         if (this._commandSet == null)
    ///             this._commandSet = new MyDataCommandSet(this);
    ///             return this._commandSet;
    ///     }
    ///     ...
    /// }
    /// </code>
    /// </example>
    /// See <see cref="CommandSet"/>
    /// <p>
    /// See <see cref="CommandSet"/> examples
    public interface ICommandable
    {
        /// <summary>
        /// Gets a command set with all supported commands and events.
        /// </summary>
        /// <returns>a command set with commands and events.</returns>
        /// See <see cref="CommandSet"/>
        CommandSet GetCommandSet();
    }
}