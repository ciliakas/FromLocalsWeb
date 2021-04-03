using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>  ReportPage()
        {
           // var reports = await _context.Reports.ToListAsync();
            // var model = new AdminViewModel
            // {
            //     Reports = reports
            // };
            // return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateReport()
        {
            var report = new Report();
            report.CreatedDate = DateTime.Now;
            report.Category = 1;
            report.Id = 4;
            report.UserId = "useris";
            report.Href = "google.com";
            _context.Reports.Add(report);
            Console.WriteLine("\n\n\n\nPridejau i db");

            return View();
        }



    }
}
