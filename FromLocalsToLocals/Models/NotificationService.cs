using FromLocalsToLocals.Database;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;

        List<Notification> notifications = new List<Notification>();

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public List<Notification> GetNotifications(string toUserId, bool getOnlyUnread)
        {
            return _context.Notifications.Where(n => n.OwnerId == toUserId).ToList();
        }
    }
}
