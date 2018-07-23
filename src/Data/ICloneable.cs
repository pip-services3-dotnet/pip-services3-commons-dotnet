namespace PipServices.Commons.Data
{
    /// <summary>
    /// Standard ICloneable interface that is missing in .NET Core.
    /// </summary>
    public interface ICloneable
    {
        object Clone();
    }
}