using FromLocalsToLocals.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Services.EF
{
    public interface INotificationService
    {
        void AddNotification(Notification notification);
        Task<bool> DeleteNotificationAsync(string userId, int? notificationId);
        List<Notification> GetNotifications(string toUserId);
    }
}
