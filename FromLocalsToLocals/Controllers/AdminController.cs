using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FromLocalsToLocals.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public List<Report> Reports { get; set; }
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(AppDbContext context, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        private async Task<AdminViewModel> ReadReviews()
        {
            var model = new AdminViewModel();
            var reports = await _context.Reports.ToListAsync() ?? new List<Report>();
            model.Reports = reports;
            return model;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReportPage()
        {

            var model = await ReadReviews();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReport(int category, string url)
        {
            var username = _signInManager.IsSignedIn(User) ? User.Identity.Name : "Guest";
            var report = new Report
            {
                Category = category,
                CreatedDate = DateTime.UtcNow,
                
                UserId = username,
               // Href = Request.GetEncodedUrl()
                //Href = HttpContext.Request.Path.Value
                //Href = HttpContext.Request.PathBase.Value
                Href = url
            };
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
            return RedirectToAction("ReportSuccess", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var item = _context.Reports.FirstOrDefaultAsync(x => x.Id == id);
            _context.Reports.Remove(item.Result);
            await _context.SaveChangesAsync();
           var model = await ReadReviews();
           return View("ReportPage", model);

        }

    }
}
