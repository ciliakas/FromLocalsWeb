using System.Collections.Generic;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;

namespace FromLocalsToLocals.Services.EF
{
    public interface INotificationService
    {
        void AddNotification(Notification notification);
        Task<bool> DeleteNotificationAsync(string userId, int? notificationId);
        List<Notification> GetNotifications(string toUserId);
    }
}