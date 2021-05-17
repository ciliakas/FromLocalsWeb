using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using FromLocalsToLocals.Utilities.Enums;

namespace FromLocalsToLocals.Contracts.Entities
{
    public class SwipeCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public VendorType VendorType { get; set; }
        public string VendorName { get; set; }
        public string Description { get; set; }
        public double Distance { get; set; }
        public double ReviewsAverage { get; set; }
    }
}