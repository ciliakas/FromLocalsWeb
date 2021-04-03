using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FromLocalsToLocals.Web.ViewModels
{
    public class LoginVM
    {
        [Required] public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")] public bool RememberMe { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RegisterVM
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required] public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ProfileVM
    {
        public ProfileVM()
        {
        }

        public ProfileVM(string email, string username, byte[] img)
        {
            Email = email;
            UserName = username;
            Image = img;
        }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string UserName { get; set; }

        public byte[] Image { get; set; }

        public IFormFile ImageFile { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordVM
    {
        [DataType(DataType.EmailAddress)] public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [MinLength(6, ErrorMessage = "Password must contain at least 6 letters!")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class BugReportVM
    {
        public string TextBug { get; set; }
    }
}