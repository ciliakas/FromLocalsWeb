using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FromLocalsToLocals.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        List<Notification> notifications = new List<Notification>();

        public NotificationController(INotificationService notificationService, UserManager<AppUser> userManager, AppDbContext context)
        {
            _context = context;
            _notificationService = notificationService;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false });
            }

            var noti = await _context.Notifications.FirstOrDefaultAsync(m => m.NotiId == id);

            if (noti == null)
            {
                return Json(new { success = false });
            }

            if (noti.OwnerId != _userManager.GetUserId(User))
            {
                return Json(new { success = false });
            }

            _context.Notifications.Remove(noti);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
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
