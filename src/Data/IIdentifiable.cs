namespace PipServices.Commons.Data
{
    public interface IIdentifiable<T>
    {
        T Id { get; set; }
    }
}