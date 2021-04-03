using System;
using FromLocalsToLocals.Contracts.Entities;

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