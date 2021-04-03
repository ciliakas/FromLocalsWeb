using System.Threading.Tasks;
using FromLocalsToLocals.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FromLocalsToLocals.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

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
        public IActionResult ReportPage()
        {
            return View();
        }

        // [HttpPost]
        // [Authorize(Roles = "Admin")]
        // public async Task<IActionResult> CreateReport()
        // {
        //
        // }



    }
}
