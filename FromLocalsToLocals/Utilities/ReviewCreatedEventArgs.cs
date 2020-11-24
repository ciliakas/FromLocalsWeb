using FromLocalsToLocals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Utilities
{
    public class ReviewCreatedEventArgs : EventArgs
    {
        public ReviewCreatedEventArgs(Review review, string vendorTitle)
        {
            Review = review;
            VendorTitle = vendorTitle;
        }

        public Review Review { get; set; }
        public string VendorTitle { get; set; }
    }
}
