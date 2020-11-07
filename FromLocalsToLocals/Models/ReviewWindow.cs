using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models
{
    public class ReviewWindow
    {
        public Vendor Vendor {get; set;}
        public IEnumerable<Review> Reviews { get; set; }

        public UserManager<AppUser> UserManager { get; set; }

    }
}
