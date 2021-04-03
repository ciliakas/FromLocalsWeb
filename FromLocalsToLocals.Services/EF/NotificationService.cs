using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using Microsoft.EntityFrameworkCore;

namespace FromLocalsToLocals.Services.EF
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public List<Notification> GetNotifications(string toUserId)
        {
            return _context.Notifications.Where(n => n.OwnerId == toUserId).ToList();
        }

        public void AddNotification(Notification notification)
        {
            try
            {
                _context.Notifications.Add(notification);
                _context.SaveChangesAsync().Wait();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteNotificationAsync(string userId, int? notificationId)
        {
            var noti = await _context.Notifications.FirstOrDefaultAsync(m => m.NotiId == notificationId);

            if (noti == null || noti.OwnerId != userId) return false;

            try
            {
                _context.Notifications.Remove(noti);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}