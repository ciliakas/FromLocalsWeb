using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FromLocalsToLocals.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;
        List<Notification> notifications = new List<Notification>();

        public NotificationController(INotificationService notificationService, UserManager<AppUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public IActionResult AllNotifications()
        {
            return View();
        }

        public JsonResult GetNotifications(bool getOnlyUnread = false)
        {
            var userId = _userManager.GetUserId(User);
            notifications = new List<Notification>();
            notifications = _notificationService.GetNotifications(userId, getOnlyUnread);
            return Json(notifications);
        }
    }
}
