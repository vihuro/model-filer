using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Utils;

namespace ModelFilter.Persistence.Utils
{
    public class NotificationService : ICustomNotification
    {
        private List<Notification> _notifications;

        public NotificationService()
        {
            _notifications = [];
        }

        public void ClearNotification()
        {
            _notifications.Clear();
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public void Handle(Notification notification)
        {
            _notifications.Add(notification);
        }

        public bool HaveNotification()
        {
            return _notifications.Any();
        }
    }
}
