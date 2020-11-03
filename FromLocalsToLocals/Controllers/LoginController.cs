using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;


        public LoginController(AppDbContext context)
        {
            _context = context;
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
                return View("ErrorView");
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

            return RedirectToAction("Login");
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
                return View("ErrorView");
            }


            return RedirectToAction("Create", "Vendors", null);
        }

    }
}
