using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Web.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FromLocalsToLocals.Web.Models.ViewModels
{
    public class VendorTypeViewModel
    {
        public PaginatedList<Vendor> Vendors { get; set; }
        public SelectList Types { get; set; }
        public SelectList OrderTypes { get; set; }
        public string OrderType { get; set; }
        public string VendorType { get; set; }
        public string SearchString { get; set; }
        public List<Vendor> NewVendors { get; set; }
    }
}