using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Services.EF;
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

        public double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        {
            double distance = 0;

            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                       + Math.Cos(lat2) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double radiusE = 6378135;
            double radiusP = 6356750;

            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                        + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            distance = radius * c;
            return distance;
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

        public async Task<List<SwipeCard>> GetSwipeCards(int Count)
        {
            var vendors = await _vendorService.GetNewVendorsAsync(Count);

            var swipecards = vendors.Select(x => new SwipeCard { Id = x.ID ,Title = x.Title, Image = x.Image, VendorType = x.VendorType, VendorName = x.UserID, Description = x.About, Distance = GetDistance(x.Longitude, x.Latitude, 25.231710, 54.719540) });
            return swipecards.OrderBy(x=>x.Distance).ToList();
        }
        public async Task<IActionResult> SwipeCard()
        {
            var swipecards = await GetSwipeCards(4);
            foreach (var x in swipecards)
            {
                Console.WriteLine("Pavadinimas " + x.Title + " Atstumas: " + x.Distance);
            }
            return View();
        }
    }
}
