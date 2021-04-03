using System.Collections.Generic;
using FromLocalsToLocals.Contracts.Entities;

namespace FromLocalsToLocals.Web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Vendor> AllVendors { get; set; }
        public IEnumerable<Vendor> PopularVendors { get; set; }
    }
}