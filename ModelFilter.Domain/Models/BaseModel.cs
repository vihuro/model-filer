namespace ModelFilter.Domain.Models
{
    public abstract class BaseModel
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
