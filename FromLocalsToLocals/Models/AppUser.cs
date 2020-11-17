using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FromLocalsToLocals.Models
{
    public class AppUser : IdentityUser
    {
        public byte[] Image { get; set; }
        public int VendorsCount { get; set; }

        public ICollection<Vendor> Vendors { get; set; }
    }
}
