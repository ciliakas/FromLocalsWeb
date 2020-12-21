using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Services.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FromLocalsToLocals.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;

        public NotificationController(INotificationService notificationService, UserManager<AppUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public JsonResult GetNotifications()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = _notificationService.GetNotifications(userId);
            
            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(notifications));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null || !await _notificationService.DeleteNotificationAsync(_userManager.GetUserId(User), id))
            {
                return BadRequest();
            }

            return Json(new {});
        }
    }
}
