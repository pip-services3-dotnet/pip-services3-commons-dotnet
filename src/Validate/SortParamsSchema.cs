using PipServices3.Commons.Convert;

namespace PipServices3.Commons.Validate
{
    public class SortParamsSchema : ArraySchema
    {
        public SortParamsSchema()
            : base(new SortFieldSchema())
        {
        }
    }
}