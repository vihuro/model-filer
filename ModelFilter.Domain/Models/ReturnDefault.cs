namespace ModelFilter.Domain.Models
{
    public class ReturnDefault<T>
    {
        public bool Sucess { get; set; } = true;
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPerPage { get; set; }
        public List<T> DataResult { get; set; }
        public List<string> Erros { get; set; }
    }
}
