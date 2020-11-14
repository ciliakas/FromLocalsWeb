using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FromLocalsToLocals.Models;
using FromLocalsToLocals.Database;
using Microsoft.EntityFrameworkCore;
using FromLocalsToLocals.Models.Services;

namespace FromLocalsToLocals.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVendorService _vendorService;

        public HomeController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            return View(await _vendorService.GetVendorsAsync(searchString, ""));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Reviews()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}