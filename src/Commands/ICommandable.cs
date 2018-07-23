namespace PipServices.Commons.Commands
{
    /// <summary>
    /// The commandable interface
    /// </summary>
    public interface ICommandable 
    {
        /// <summary>
        /// Gets the command set.
        /// </summary>
        CommandSet GetCommandSet();
    }
}