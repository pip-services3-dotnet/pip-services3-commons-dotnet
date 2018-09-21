using PipServices.Commons.Convert;

namespace PipServices.Commons.Validate
{
    /// <summary>
    /// Schema to validate ProjectionParams.
    /// </summary>
    /// See <see cref="ProjectionParams"/>
    public class ProjectionParamsSchema : ArraySchema
    {
        /// <summary>
        /// Creates a new instance of validation schema.
        /// </summary>
        public ProjectionParamsSchema()
            : base(TypeCode.String)
        {
        }
    }
}