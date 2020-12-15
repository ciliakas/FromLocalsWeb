using FromLocalsToLocals.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models
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
            catch(Exception ex)
            { throw ex; }
        }

        public async Task DeleteAllNotificationsAsync(AppUser user)
        {
            try
            {
                _context.Notifications.RemoveRange(_context.Notifications.Where(n => n.OwnerId == user.Id));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { throw ex; }

        }

        public async Task DeleteNotificationAsync(int notificationId)
        {
            try
            {
                _context.Notifications.Remove(_context.Notifications.FirstOrDefault(n => n.NotiId == notificationId));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { throw ex; }
            
        }

        public async Task DeleteNotificationAsync(Notification notification)
        {
            try
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex; 
            }
       
        }

    }
}
