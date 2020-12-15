using FromLocalsToLocals.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Services.EF
{
    public interface INotificationService
    {
        void AddNotification(Notification notification);
        Task DeleteNotificationAsync(int notificationId);
        Task DeleteNotificationAsync(Notification notification);
        Task DeleteAllNotificationsAsync(AppUser user);

        List<Notification> GetNotifications(string toUserId);
    }
}
