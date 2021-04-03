using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Utilities.Enums;
using Microsoft.AspNetCore.Http;

namespace FromLocalsToLocals.Web.ViewModels
{
    public class CreateEditVendorVM
    {
        public CreateEditVendorVM()
        {
        }

        public CreateEditVendorVM(Vendor vendor, List<WorkHours> workHours)
        {
            ID = vendor.ID;
            Title = vendor.Title;
            About = vendor.About;
            Address = vendor.Address;
            VendorType = vendor.VendorType;
            VendorHours = workHours;
        }

        public int ID { get; set; }

        [Required] [Display(Name = "Title")] public string Title { get; set; }

        [Display(Name = "About")] public string About { get; set; }

        public string Link { get; set; }

        [Required] [Display(Name = "Address")] public string Address { get; set; }

        [Required]
        [Display(Name = "VendorType")]
        public VendorType VendorType { get; set; }

        public IFormFile Image { get; set; }

        public List<WorkHours> VendorHours { get; set; }

        public void SetValuesToVendor(Vendor vendor)
        {
            vendor.Title = Title;
            vendor.About = About;
            vendor.Address = Address;
            vendor.VendorType = VendorType;

            if (Image != null)
                using (var target = new MemoryStream())
                {
                    Image.CopyTo(target);
                    vendor.Image = target.ToArray();
                }
        }
    }
}