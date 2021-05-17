using System;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Services.EF;
using FromLocalsToLocals.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FromLocalsToLocals.Web.Controllers
{
    public class SwipeCardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IVendorService _vendorService;

        public SwipeCardController(UserManager<AppUser> userManager, AppDbContext context, IVendorService vendorService)
        {
            _context = context;
            _vendorService = vendorService;
        }

        public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            var d1 = latitude * (Math.PI / 180.0);
            var num1 = longitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return (6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)))) / 1000;
        }

        public string GetUserNameById(string UserID)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == UserID);
            return user != null ? user.UserName : "Vendor";
        }

        public async Task<SwipeCardVM> GetSwipeCards()
        {
            var model = new SwipeCardVM();
            var vendors = await _vendorService.GetVendorsAsync();

            var swipeCards = vendors.Select(x => new SwipeCard
            {
                Id = x.ID,
                Title = x.Title,
                Image = x.Image,
                VendorType = x.VendorType,
                VendorName = GetUserNameById(x.UserID),
                Description = x.About,
                Distance = GetDistance(x.Longitude, x.Latitude, 25.273820, 54.676050),
                ReviewsAverage = x.ReviewsAverage
            });

            model.SwipeCards = swipeCards.OrderBy(x => x.Distance).ToList();
            return model;
        }
        public async Task<IActionResult> SwipeCard()
        {
            var model = await GetSwipeCards();
            return View(model);
        }
    }
}
