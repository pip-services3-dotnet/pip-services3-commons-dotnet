using PipServices.Commons.Convert;

namespace PipServices.Commons.Validate
{
    public class SortParamsSchema : ArraySchema
    {
        public SortParamsSchema()
            : base(new SortFieldSchema())
        {
        }
    }
}