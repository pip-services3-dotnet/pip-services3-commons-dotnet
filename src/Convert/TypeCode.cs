namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Codes for the data types that can be converted using TypeConverter.
    /// </summary>
    /// See <see cref="TypeConverter"/>
    public enum TypeCode
    {
        Unknown,
        String,
        Boolean,
        Integer,
        Long,
        Float,
        Double,
        DateTime,
        Duration,
        Object,
        Enum,
        Array,
        Map
    }
}
