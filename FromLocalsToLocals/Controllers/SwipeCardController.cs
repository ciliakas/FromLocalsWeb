using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Services.EF;
using FromLocalsToLocals.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FromLocalsToLocals.Web.Controllers
{
    public class SwipeCardController : Controller
    {
        private readonly IVendorService _vendorService;

        public SwipeCardController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            var d1 = latitude * (Math.PI / 180.0);
            var num1 = longitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return (6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))))/1000;
        }

        public async Task<SwipecardVM> GetSwipeCards()
        {
            var model = new SwipecardVM();
            var vendors = await _vendorService.GetVendorsAsync();

            var swipecards = vendors.Select(x => new SwipeCard { Id = x.ID ,Title = x.Title, Image = x.Image, VendorType = x.VendorType, VendorName = x.UserID, Description = x.About, Distance = GetDistance(x.Longitude, x.Latitude, 25.231710, 54.719540) });

            model.Swipecards = swipecards.OrderBy(x => x.Distance).ToList();
            return model;
        }
        public async Task<IActionResult> SwipeCard()
        {
            var model = await GetSwipeCards();
            Console.WriteLine(model.Swipecards[0].Title);
            Console.WriteLine(model.Swipecards[0].Distance);
            return View(model);
        }
    }
}
