using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FromLocalsToLocals.Models
{
    public class VendorTypeViewModel
    {
        public List<Vendor> Vendors { get; set; }
        public SelectList Types { get; set; }
        public string VendorType { get; set; }
        public string SearchString { get; set; }
    }
}