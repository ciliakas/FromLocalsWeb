﻿using FromLocalsToLocals.Utilities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FromLocalsToLocals.Models.ViewModels
{
    public class CreateEditVendorVM
    {
        public CreateEditVendorVM(){}

        public CreateEditVendorVM(Vendor vendor)
        {
            ID = vendor.ID;
            Title = vendor.Title;
            About = vendor.About;
            Address = vendor.Address;
            VendorType = vendor.VendorType;
        }

        public int ID { get; set; }
        [Required]
        public string Title { get; set; }

        public string About { get; set; }

        [Required]
        public string Address { get; set; }

        [Required] public VendorType VendorType { get; set; }

        public IFormFile Image { get; set; }


        public void SetValuesToVendor(Vendor vendor)
        {
            vendor.Title = Title;
            vendor.About = About;
            vendor.Address = Address;
            vendor.VendorType = VendorType;

            if (Image != null)
            {
                using (var target = new MemoryStream())
                {
                    Image.CopyTo(target);
                    vendor.Image = target.ToArray();
                }
            }

        }

    }
}