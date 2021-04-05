using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FromLocalsToLocals.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public List<Report> Reports { get; set; }

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReportPage()
        {
            var model = new AdminViewModel();
            var reports = await _context.Reports.ToListAsync();
            model.Reports = reports;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReportPage(AdminViewModel model)
        {
            var report = new Report
            {
                Category = model.Category,
                CreatedDate = DateTime.UtcNow,
                UserId = "Guest",
                Href = Request.GetEncodedUrl()
            };
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
            // var reports = await _context.Reports.ToListAsync();
            // var model = new AdminViewModel
            // {
            //     Reports = reports
            // };
            // return View(model);
            return await ReportPage();
        }

    }
}
