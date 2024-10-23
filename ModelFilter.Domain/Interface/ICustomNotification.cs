using ModelFilter.Domain.Utils;

namespace ModelFilter.Domain.Interface
{
    public interface ICustomNotification
    {
        bool HaveNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
        void ClearNotification();
    }
}
