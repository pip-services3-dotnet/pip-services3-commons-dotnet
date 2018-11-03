using PipServices3.Commons.Convert;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Schema to validate FilterParams.
    /// </summary>
    /// See <see cref="FilterParams"/>
    public class FilterParamsSchema : MapSchema
    {
        /// <summary>
        /// Creates a new instance of validation schema.
        /// </summary>
        public FilterParamsSchema()
            : base(TypeCode.String, null)
        {
        }
    }
}