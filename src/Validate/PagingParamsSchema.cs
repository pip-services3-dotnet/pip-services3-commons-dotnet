using PipServices3.Commons.Convert;
using System;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Schema to validate PagingParams.
    /// </summary>
    /// See <see cref="PagingParams"/>
    public class PagingParamsSchema : ObjectSchema
    {
        /// <summary>
        /// Creates a new instance of validation schema.
        /// </summary>
        public PagingParamsSchema()
        {
            WithOptionalProperty("skip", typeof(long));
            WithOptionalProperty("take", typeof(long));
            WithOptionalProperty("total", Convert.TypeCode.Boolean);
        }
    }
}