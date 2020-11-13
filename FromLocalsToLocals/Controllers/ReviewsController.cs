using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using FromLocalsToLocals.Utilities;

namespace FromLocalsToLocals.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ReviewsController(AppDbContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _hubContext = hubContext;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            var id = GetVendorID();

            var model = new ReviewViewModel();
            var reviews = await _context.Reviews.Where(x => x.VendorID == id).ToListAsync();

            var vendor = await _context.Vendors.FindAsync(id);
            vendor.UpdateReviewsCount(_context);
            model.Vendor = vendor;

            model.Reviews = from review in reviews
                             join user in _context.Users on review.SenderUsername equals user.UserName into temp

                             from leftTable in temp.DefaultIfEmpty()
                             select new Review{
                                 VendorID = review.VendorID, 
                                 SenderUsername = review.SenderUsername,
                                 CommentID = review.CommentID,
                                 Text = review.Text,
                                 Stars = review.Stars,
                                 Date = review.Date,
                                 Reply = review.Reply,
                                 SenderImage = leftTable?.Image ?? null
                             };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reviews(ReviewViewModel model)
        {
            var id = GetVendorID();
            var vendor = await _context.Vendors.FindAsync(id);
            var userLoggedIn = _signInManager.IsSignedIn(User);

            model.Reviews = await _context.Reviews.Where(x => x.VendorID == id).ToListAsync();
            model.Vendor = vendor;

            if (vendor == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["comment"]))
            {
                var review = new Review();
                review.VendorID = id;
                review.CommentID = int.Parse(Request.Form["listItemCount"]);

                if (userLoggedIn)
                {
                    var user = await _userManager.GetUserAsync(User);
                    review.SenderUsername = user.UserName;
                }

                else
                {
                    review.SenderUsername = "Anonimas";
                }

                review.Text = Request.Form["comment"];
                review.Stars = int.Parse(Request.Form["starRating"]);
                review.Date = DateTime.Now.ToString("yyyy-MM-dd");
                review.Reply = "";

                _context.Reviews.Add(review);
                _context.SaveChanges();
                vendor.UpdateReviewsCount(_context);

                model.Reviews = await _context.Reviews.Where(x => x.VendorID == id).ToListAsync();
                model.Vendor = vendor;

                //Notify vendor owner that someone commented on his shop
                var notification = new Notification
                {
                    OwnerId = _context.Vendors.FirstOrDefault(v => v.ID == GetVendorID()).UserID,
                    VendorId = id,
                    IsRead = false,
                    CreatedDate = DateTime.Now,
                    NotiBody = $"{review.SenderUsername} gave {review.Stars} stars to '{vendor.Title}'.",
                    Url = HttpContext.Request.Path.Value
                };

                _context.Notifications.Add(notification);
                _context.SaveChanges();

                await _hubContext.Clients.All.SendAsync("displayNotification", "");

                return View(model);
            }
            return View(model);
        }

        private int GetVendorID()
        {
            string path = HttpContext.Request.Path.Value;
            return int.Parse(path.Remove(0, 26));
        }

        /* STILL NEEDED
        private void PostComment(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var replyBox = ((Grid) button.Parent).FindName("ReplyTextBox") as TextBox;

            var comment = ((Grid) button.Parent).FindName("UserComment") as TextBlock;

            var replyGrid = ((Grid) button.Parent).FindName("ReplyGrid") as Grid;
            var commentGrid = ((Grid) button.Parent).FindName("CommentGrid") as Border;

            replyGrid.Visibility = Visibility.Collapsed;
            commentGrid.Visibility = Visibility.Visible;


            comment.Text = _vendor.Title + "\n" + "\n" + replyBox.Text + "\n" + DateTime.Now.ToString("yyyy-MM-dd");

            // getting the index of the pressed POST button
            var index = GetIndex(button);

            using var db = new AppDbContext();
            var user = db.Reviews.SingleOrDefault(x => (x.VendorID == _vendor.ID) && (x.CommentID == index));
            user.Reply = comment.Text;
            db.SaveChanges();
        }

        private int GetIndex(FrameworkElement element)
        {
            return RView.Items.IndexOf(element.DataContext);
        }
    } 
         */
    }
}
