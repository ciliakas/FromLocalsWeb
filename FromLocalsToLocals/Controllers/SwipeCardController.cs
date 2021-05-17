﻿using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
        private readonly UserManager<AppUser> _userManager;

        public SwipeCardController(UserManager<AppUser> userManager, AppDbContext context, IVendorService vendorService)
        {
            _userManager = userManager;
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

            return (6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))))/1000;
        }

        public string GetUserNameById(string UserID)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == UserID);
            return user.UserName;
        }

        public async Task<SwipecardVM> GetSwipeCards()
        {
            var model = new SwipecardVM();
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

            model.Swipecards = swipeCards.OrderBy(x => x.Distance).ToList();
            return model;
        }
        public async Task<IActionResult> SwipeCard()
        {
            var model = await GetSwipeCards();
            foreach (var x in model.Swipecards)
            {
                Console.WriteLine("user: " + x.VendorName + " Service: " + x.Title + " Distance: " + x.Distance);
            }
            return View(model);
        }
    }
}
