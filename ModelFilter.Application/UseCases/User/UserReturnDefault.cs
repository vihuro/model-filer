namespace ModelFilter.Application.UseCases.User
{
    public class UserReturnDefault
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string? Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
