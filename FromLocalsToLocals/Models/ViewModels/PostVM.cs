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

        [Required]
        public string PostText { get; set; }

        [Required]
        public string SelectedVendorTitle { get; set; }

        public byte[] VendorImage { get; set; }

    }

    public enum Tab
    {
        AllFeed,
        MyFeed
    }

}
