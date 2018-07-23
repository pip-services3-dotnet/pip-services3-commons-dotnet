using PipServices.Commons.Convert;
using System;

namespace PipServices.Commons.Validate
{
    public class PagingParamsSchema : ObjectSchema
    {
        public PagingParamsSchema()
        {
            WithOptionalProperty("skip", typeof(long));
            WithOptionalProperty("take", typeof(long));
            WithOptionalProperty("total", Convert.TypeCode.Boolean);
        }
    }
}