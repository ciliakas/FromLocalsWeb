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

        public async Task<List<SwipeCard>> GetSwipeCards(int Count)
        {
            var vendors = await _vendorService.GetNewVendorsAsync(Count);
            var swipecards = vendors.Select(x => new SwipeCard { Id = x.ID ,Title = x.Title, Image = x.Image, VendorType = x.VendorType, VendorName = x.UserID, Description = x.About });
            return swipecards.ToList();
        }
        public async Task<IActionResult> SwipeCard()
        {
            var swipecards = await GetSwipeCards(4);
            return View();
        }
    }
}
