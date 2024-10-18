namespace ModelFilter.Domain.Utils.Filters
{
    public class FilterBaseDto
    {
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int MaxPerPage { get; set; } = 100;
        public List<Filter> Filters { get; set; }
        public List<MultiSort> MultiSort { get; set; }

    }
    public class Filter
    {
        public string Field { get; set; }
        public object Value { get; set; }
        public string Operation { get; set; }
    }
    public class MultiSort
    {
        public string Field { get; set; }
        public string Value { get; set; }
    }
}
