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
using Microsoft.AspNetCore.Authorization;
using FromLocalsToLocals.ViewModels;
using SuppLocals;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NToastNotify;
using System.Threading;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;

namespace FromLocalsToLocals.Controllers
{
    public class HomeController : Controller
    {
        private readonly IToastNotification _toastNotification;

        private readonly IVendorService _vendorService;

        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IStringLocalizer<HomeController> localizer, IVendorService vendorService, IToastNotification toastNotification)
        {
            _localizer = localizer;
            _vendorService = vendorService;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            return View(await _vendorService.GetVendorsAsync(searchString, ""));
        }

        public IActionResult ReportBug()
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ReportBug(BugReportVM model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.TextBug)) {
                    await Execute();
                    _toastNotification.AddSuccessToastMessage(_localizer["Report message send succesfully!"]);
                    return View("Index");
                }

                else
                {
                    _toastNotification.AddErrorToastMessage(_localizer["Report message can not be empty!"]);
                    return View();
                }
            }

            async Task Execute()
            {
                var email = Config.email;
                var key = Config.Send_Grid_Key;
                var client = new SendGridClient(key);

                var from = new EmailAddress("fromlocalstolocals@gmail.com", "Bug reporter");
                var subject = "Found bug";
                var to = new EmailAddress(email , "Dear User");
                var plainTextContent = "";

                var htmlContent = "<!DOCTYPE html><html><head><meta charset=\"UTF-8\"></head><body>" + model.TextBug;
      
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }

  

            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}