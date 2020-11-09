using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using FromLocalsToLocals.ViewModels;
using Geocoding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using SuppLocals;

namespace FromLocalsToLocals.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly NToastNotify.IToastNotification _toastNotification;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
                                 AppDbContext context, ILogger<AccountController> logger, NToastNotify.IToastNotification toastNotification)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _toastNotification = toastNotification;
        }

  
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Email = model.Email,
                    UserName = model.Username
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }


                ModelState.AddModelError(string.Empty,"Invalid Login Attempt");

            }

            return View(model);
        }


        public IActionResult Profile()
        {
            var userId = _userManager.GetUserId(User);
            var user = _context.Users.Single(x => x.Id == userId);

            var model = GetNewProfileVM(user);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(string submitBtn, ProfileVM model)
        {
            switch (submitBtn)
            {
                case "picName":
                    return await PicNameChange(model);
                case "password":
                    return await ChangePassword(model);
                default:
                    return View();
            }
        }
        private async Task<IActionResult> PicNameChange(ProfileVM model)
        {
            var userId = _userManager.GetUserId(User);
            var user = _context.Users.Single(x => x.Id == userId);
            var oldModel = GetNewProfileVM(user);
            var resultsList = new List<IdentityResult>();

            if (model.UserName != user.UserName)
            {
                if (model.UserName == "")
                {
                    ModelState.FirstOrDefault(x => x.Key == nameof(model.UserName)).Value.RawValue = user.UserName;
                    ModelState.AddModelError("", $"Username cannot be empty");
                }
                else if (!_context.Users.Any(x => x.UserName == model.UserName))
                {
                    var resUser = await _userManager.SetUserNameAsync(user, model.UserName);
                    resultsList.Add(resUser);
                }
                else
                {
                    ModelState.FirstOrDefault(x => x.Key == nameof(model.UserName)).Value.RawValue = user.UserName;
                    ModelState.AddModelError("", $"Username '{model.UserName}' is already taken.");
                }
            }
            if (model.Email != user.Email)
            {
                if (model.Email == "")
                {
                    ModelState.FirstOrDefault(x => x.Key == nameof(model.Email)).Value.RawValue = user.Email;
                    ModelState.AddModelError("", $"Email cannot be empty");
                }
                else if (!_context.Users.Any(x => x.Email == model.Email))
                {
                    var resEmail = await _userManager.SetEmailAsync(user, model.Email);
                    resultsList.Add(resEmail);
                }
                else
                {
                    ModelState.FirstOrDefault(x => x.Key == nameof(model.Email)).Value.RawValue = user.Email;
                    ModelState.AddModelError("", $"Email '{model.Email}' is already in use.");
                }
            }
            if (model.ImageFile != null)
            {
                if (model.ImageFile.Length > 0)
                {
                    using (var target = new MemoryStream())
                    {
                        model.ImageFile.CopyTo(target);
                        user.Image = target.ToArray();
                        model.Image = user.Image;
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
            }
            var errors = GetErrors(resultsList);
            if (!errors.IsNullOrEmpty())
            {
                errors.ForEach(e => ModelState.AddModelError("", e));
            }

            if (ModelState.ErrorCount == 0)
                _toastNotification.AddSuccessToastMessage("Changes saved successfully");

            return Profile();
        }
        private async Task<IActionResult> ChangePassword(ProfileVM model)
        {
            if (string.IsNullOrWhiteSpace(model.OldPassword) || string.IsNullOrWhiteSpace(model.NewPassword) || string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Please, fill all fields");
                return Profile();
            }
            if (model.NewPassword.Length < 6)
            {
                ModelState.AddModelError("", "Password must be at least 6 characters long");
                return Profile();
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match");
                return Profile();
            }

            var user = await _userManager.GetUserAsync(User);

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return Profile();
            }

            await _signInManager.RefreshSignInAsync(user);
            _toastNotification.AddSuccessToastMessage("Password changed successfully");

            return Profile();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Register") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordVM model)
        {
   
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("Register", "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code , model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
               

                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("Register");
                }
                
          
                Execute().Wait();
                return View("ForgotPasswordConfirmation");
            }

            async Task Execute()
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var key = Config.Send_Grid_Key;
                var client = new SendGridClient(key);

                var from = new EmailAddress("fromlocalstolocals@gmail.com", "Example User");
                var subject = "Forgot Password Confirmation";
                var to = new EmailAddress(model.Email, "Dear User");
                var plainTextContent = "";

                var code = await _userManager.GeneratePasswordResetTokenAsync(user); 
               


                var callbackUrl = Url.Action("ResetPassword", "Account",
                new { user = user , code = code }, protocol: Request.Scheme); 



                var htmlContent = "<!DOCTYPE html><html><head><meta charset=\"UTF-8\"></head><body>" +
                                  "Please confirm your account by clicking this link: <a href =\""
                                                 + callbackUrl + "\">link</a> </body></html>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }

            return View();
        }

        #region Helpers

        private List<string> GetErrors(List<IdentityResult> results)
        {
            var tempAns = new List<string>();

            foreach (var r in results)
            {
                if (!r.Succeeded)
                {
                    r.Errors.ForEach(x => tempAns.Add(x.Description));
                }
            }
            return tempAns;
        }

        public static ProfileVM GetNewProfileVM(AppUser user)
        {
            return new ProfileVM(user.Email, user.UserName, user.Image);
        }
        #endregion



    }
}