using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FromLocalsToLocals.Models;

namespace FromLocalsToLocals.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var listOfVendors = new List<Vendor>
            {
                new Vendor(56.428075, 22.9676068),
                new Vendor(56.428075, 22.9676068),
                new Vendor(56.428075, 22.9676068),
                new Vendor(56.428075, 22.9676068),
                new Vendor(56.428075, 22.9676068),
                new Vendor(56.428075, 22.9676068),
                new Vendor(56.428075, 22.9676068),
                new Vendor(56.428075, 22.9676068),
                new Vendor(56.428075, 22.9676068)
            };

            ViewBag.listas = listOfVendors;


            return View(listOfVendors);
        }

        public IActionResult Privacy()
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
