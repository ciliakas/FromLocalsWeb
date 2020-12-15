using FromLocalsToLocals.Contracts.Entities;
using System;

namespace FromLocalsToLocals.Contracts.Events
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
