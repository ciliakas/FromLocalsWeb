using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Contracts.Events;
using FromLocalsToLocals.Web.ViewModels;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Web.Utilities;
using FromLocalsToLocals.Services.EF;

namespace FromLocalsToLocals.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<NotiHub> _hubContext;
        private readonly IReviewsService _reviewsService;
        private readonly INotificationService _notificationService;
        private readonly IVendorService _vendorService;
        private readonly AppDbContext _context;

        public ReviewsController(UserManager<AppUser> userManager, IHubContext<NotiHub> hubContext,
            IReviewsService reviewsService, INotificationService notificationService,
            IVendorService vendorService , AppDbContext context)
        {
            _vendorService = vendorService;
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
            _reviewsService = reviewsService;
            _notificationService = notificationService;

            PublisherSingleton.Instance.ReviewCreatedEvent+=NotifyUserWithNewReview;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            var id = GetVendorID();

            var vendor = await _vendorService.GetVendorAsync(id);
            var reviews = await _reviewsService.GetReviewsAsync(id);
            var users = await _userManager.Users.ToListAsync();

            users.Add(new AppUser { UserName = "Anonimas" });

            var model = new ReviewViewModel
            {
                Vendor = vendor,
                Reviews = reviews.Join(users,
                    rev => rev.SenderUsername,
                    kit => kit.UserName,
                    (rev, kit) => Tuple.Create(rev, kit.Image))
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reviews(ReviewViewModel model)
        {
            var id = GetVendorID();
            var vendor = await _vendorService.GetVendorAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (vendor == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["vendorReply"]))
            {
                var index = int.Parse(Request.Form["postReview"]);

                await _reviewsService.AddReplyAsync(id, index, Request.Form["vendorReply"], vendor.Title); 
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["comment"]) && (int.Parse(Request.Form["starRating"]) != 0))
            {
                var commentId = int.Parse(Request.Form["listItemCount"]);
                var stars     = int.Parse(Request.Form["starRating"]);
                var userName = (user != null) ? user.UserName : "Anonimas";
                new Review(id, commentId, userName, Request.Form["comment"], stars , vendor.Title);
            }

            return await Reviews();
        }

        private async Task NotifyUserWithNewReview(object sender , ReviewCreatedEventArgs e )
        {
            var id = GetVendorID();
            var ownerID = _context.Vendors.FirstOrDefault(v => v.ID == id).UserID;

            var notification = new Notification
            {
                OwnerId = ownerID,
                VendorId = id,
                CreatedDate = DateTime.UtcNow,
                Review = e.Review,
                NotiBody = $"{e.Review.SenderUsername} gave {e.Review.Stars} stars to '{e.VendorTitle}'.",
                Url = HttpContext.Request.Path.Value
            };


            _notificationService.AddNotification(notification);
            await _hubContext.Clients.Users(ownerID).SendAsync("displayNotification", "");
        }

        private int GetVendorID()
        {
            string path = HttpContext.Request.Path.Value;
            return int.Parse(path.Remove(0, 26));
        }
    }
}
