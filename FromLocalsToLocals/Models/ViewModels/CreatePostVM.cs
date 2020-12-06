using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models.ViewModels
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
