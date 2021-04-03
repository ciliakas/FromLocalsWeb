using System;
using System.Collections.Generic;
using FromLocalsToLocals.Contracts.Entities;

namespace FromLocalsToLocals.Web.ViewModels
{
    public class ReviewViewModel
    {
        public Vendor Vendor { get; set; }
        public IEnumerable<Tuple<Review, byte[]>> Reviews { get; set; }
    }
}