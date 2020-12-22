using FromLocalsToLocals.Contracts.Entities;
using System.Collections.Generic;


namespace FromLocalsToLocals.Web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Vendor> AllVendors { get; set; }
        public IEnumerable<Vendor> PopularVendors { get; set; }
    }
}
