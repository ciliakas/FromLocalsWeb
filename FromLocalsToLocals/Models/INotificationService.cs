using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models
{
    public interface INotificationService
    {
        List<Notification> GetNotifications(string toUserId, bool getOnlyUnread);
    }
}
