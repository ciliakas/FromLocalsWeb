using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models.ViewModels
{
    public class PostVM
    {

        public Tab ActiveTab { get; set; }

        public AppUser User { get; set; }
        public IEnumerable<Tuple<Post,byte[]>> Posts { get; set; } 

        public Vendor SelectedVendor { get; set; }

        public bool DisplayInDetails { get; set; } = false;

        [Required]
        public string PostText { get; set; }

        [Required]
        public string SelectedVendorTitle { get; set; }

        public byte[] VendorImage { get; set; }

        public IFormFile Image { get; set; }

        public string PostBackUrl { get; set; }
    }

    public enum Tab
    {
        AllFeed,
        MyFeed,
        VendorFeed
    }

}
