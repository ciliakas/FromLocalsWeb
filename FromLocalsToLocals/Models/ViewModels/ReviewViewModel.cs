using FromLocalsToLocals.Contracts.Entities;
using System;
using System.Collections.Generic;


namespace FromLocalsToLocals.Web.Models.ViewModels
{
    public class ReviewViewModel
    {
        public Vendor Vendor { get; set; }
        public IEnumerable<Tuple<Review,byte[]>> Reviews { get; set; }
    }
}

