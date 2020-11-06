using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using System;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace FromLocalsToLocals.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly Vendor _vendor;
        private readonly UserManager<AppUser> _userManager;

        public ReviewsController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reviews(Review review, int id)
        {
            return View(await _context.Reviews.Where(x => x.VendorID == id).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reviews(Review review, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor =  _context.Vendors
                .FirstOrDefault(m => m.ID == id);
            if (vendor == null)
            {
                return NotFound();
            }


            

            return View(_context.Reviews.Where(x => x.VendorID == id).ToList());
        }

        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.ID == id);
        }


        private IEnumerable<Review> reviews { get; set; }
        //private readonly List<string> _stars = new List<string> { "☆☆☆☆☆", "★☆☆☆☆", "★★☆☆☆", "★★★☆☆", "★★★★☆", "★★★★★" };



        /*
         private readonly List<string> _stars = new List<string> {"☆☆☆☆☆", "★☆☆☆☆", "★★☆☆☆", "★★★☆☆", "★★★★☆", "★★★★★"};
        private readonly User _user;
        private readonly Vendor _vendor;

        private List<Review> _reviews;

        public ReviewsWindow(Vendor vendor, User activeUser)
        {
            // by default
            InitializeComponent();
            DataContext = this;
            _vendor = vendor;
            _user = activeUser;


            if (_vendor.UserID == _user.ID)
            {
                CanComment = Visibility.Hidden;
                Grid.SetRowSpan(RView, 3);
            }
            else
            {
                CanComment = Visibility.Visible;
            }

            PopulateData();
        }

        public Visibility CanComment { get; set; }

        // Adding user comment when button pressed
        private void ConfirmClicked(object sender, RoutedEventArgs e)
        {
            var comment = comments.Text;
            ConfirmError.Visibility = Visibility.Hidden;

            // counter for comments to replies
            var i = RView.Items.Count;

            if (string.IsNullOrWhiteSpace(comment))
            {
                ConfirmError.Visibility = Visibility.Visible;
                return;
            }

            var r = new Review
            {
                VendorID = _vendor.ID,
                CommentID = i,
                SenderUsername = _user.Username,
                Text = comment,
                Stars = Rating.RatingValue,
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                Reply = ""
            };

            using (var db = new AppDbContext())
            {
                db.Reviews.Add(r);
                db.SaveChanges();
            }

            PopulateData();
            comments.Clear();
        }

        private void UpdateRatingCounts()
        {
            ZeroRating.Text = _vendor.ReviewsCount[0].ToString();
            OneRating.Text = _vendor.ReviewsCount[1].ToString();
            TwoRating.Text = _vendor.ReviewsCount[2].ToString();
            ThreeRating.Text = _vendor.ReviewsCount[3].ToString();
            FourRating.Text = _vendor.ReviewsCount[4].ToString();
            FiveRating.Text = _vendor.ReviewsCount[5].ToString();
            Average.Text = _vendor.Average().ToString("0.0");
        }

        private void PopulateData()
        {
            RView.Items.Clear();

            _vendor.UpdateReviewsCount();

            using var db = new AppDbContext();
            _reviews = (from r in db.Reviews
                           where r.VendorID == _vendor.ID
                           select r).ToList();

            foreach (var review in _reviews)
            { 
                var comment = review.SenderUsername + " " + _stars[review.Stars] + "\n" + review.Text + "\n" + review.Date;

                RView.Items.Add(new Item {Text = comment, Response = review.Reply});
            }

            UpdateRatingCounts();
        }

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
