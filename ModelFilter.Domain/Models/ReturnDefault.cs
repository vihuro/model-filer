namespace ModelFilter.Domain.Models
{
    public class ReturnDefault<T>
    {
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPerPage { get; set; }
        public List<T> DataResult { get; set; }
    }
}
