using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using FromLocalsToLocals.Utilities.Enums;

namespace FromLocalsToLocals.Contracts.Entities
{
    public class SwipeCard
    {
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public VendorType VendorType { get; set; }
        public string VendorName { get; set; }
        public string Description { get; set; }
    }
}