using FromLocalsToLocals.Contracts.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FromLocalsToLocals.Web.ViewModels
{
    public class CreatePostVM
    {
        public AppUser User { get; set; }

        [Required]
        public string VendorTitle { get; set; }

        [Required]
        public string Text { get; set; }

        public IFormFile Image{get;set;}

        public string PostBackUrl { get; set; }

        public bool DisplayInDetails { get; set; }

    }
}
