﻿using System;
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

        public NotificationController(INotificationService notificationService, UserManager<AppUser> userManager, AppDbContext context)
        {
            _context = context;
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public JsonResult GetNotifications()
        {
            var userId = _userManager.GetUserId(User);
            return Json(_notificationService.GetNotifications(userId));
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

            try
            {
                await _notificationService.DeleteNotificationAsync(noti);
            }
            catch
            {
                return Json(new { success = false });
            }
            
            return Json(new { success = true });
        }

    }
}