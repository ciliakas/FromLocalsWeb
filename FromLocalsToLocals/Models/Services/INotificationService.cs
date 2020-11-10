using System.Collections.Generic;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models
{
    public interface INotificationService
    {
        Task AddNotificationAsync(Notification notification);
        Task DeleteNotificationAsync(int notificationId);
        Task DeleteNotificationAsync(Notification notification);
        Task DeleteAllNotificationsAsync(AppUser user);

        List<Notification> GetNotifications(string toUserId);
    }
}
