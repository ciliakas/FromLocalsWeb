using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FromLocalsToLocals.Models;
using FromLocalsToLocals.Database;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Identity;
using System;

namespace FromLocalsToLocals.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Map()
        {
            return View(await _context.Vendors.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser()
        {
            var email = Request.Form["Email"];
            var username = Request.Form["Username"];
            string password = Request.Form["Password"];
            var confpassword = Request.Form["confPassword"];

            var hashedPassword = new PasswordHasher<object?>().HashPassword(null, password);

            using var db = _context;

            var usersList = db.Users.ToList();
            if (usersList.FirstOrDefault(x => x.Username == username) != null || username == "Anonimas")
            {
                return RedirectToAction("Privacy");
            }

            var newUser = new User()
            {
                Username = username,
                HashedPsw = hashedPassword,
                VendorsCount = 0,
                Email = email
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            return RedirectToAction("Map");
        }

        [HttpPost]
        public IActionResult LoginUser()
        {
            using var db = _context;

            var username = Request.Form["UsernameLogin"];
            string password = Request.Form["PasswordLogin"];

            var hashedPassword = new PasswordHasher<object?>().HashPassword(null, password);
            var isPasswordHashGood = false;

            var passwordVerificationResult = new PasswordHasher<object?>().VerifyHashedPassword(null, hashedPassword, password);
            isPasswordHashGood = passwordVerificationResult switch
            {
                PasswordVerificationResult.Failed => false,
                PasswordVerificationResult.Success => true,
                PasswordVerificationResult.SuccessRehashNeeded => true,
                _ => throw new ArgumentOutOfRangeException(),
            };

            var usersList = db.Users.ToList();
            var user = usersList.SingleOrDefault(x => (x.Username == username) & isPasswordHashGood);

            if (user == null)
            {
                return RedirectToAction("Privacy");
            }


            
            return RedirectToAction("Map");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
