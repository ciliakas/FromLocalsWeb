﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using SuppLocals;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using System;
using Microsoft.VisualBasic;

namespace FromLocalsToLocals.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;
        public ActionResult Index()
        {
            return View();
        }

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        public class Comment
        {
            public string Text { get; set; }
            public string SenderID { get; set; }
            public string Date { get; set; }
            public string Response { get; set; }
        }

        [HttpPost]
        public JsonResult PostReview(Comment comment)
        {
            JsonResult result = new JsonResult();

            try
            {
                var review = new Comment();
                review.Text = comment.Text;
                //review.SenderID = comment.SenderID;
                review.Date = DateTime.Now.Date.ToString("yyyy/MM/dd");

                result.Value = new { Success = }
            }
            catch (Exception e)
            {

            }
          

            return result;
        }


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
