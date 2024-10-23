namespace ModelFilter.Domain.Utils
{
    public class Notification
    {
        public string Message { get; set; }
        public Notification(string message)
        {
            Message = message;
        }
    }
}
