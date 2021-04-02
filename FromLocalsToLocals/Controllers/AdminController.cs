using Microsoft.AspNetCore.Mvc;


namespace FromLocalsToLocals.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ReportPage()
        {
            return View();
        }
    }
}
