namespace PipServices.Commons.Data
{
    public class SortField
    {
        public SortField(string name = null, bool ascending = true)
        {
            Name = name;
            Ascending = ascending;
        }

        public string Name { get; set; }
        public bool Ascending { get; set; }
    }
}
