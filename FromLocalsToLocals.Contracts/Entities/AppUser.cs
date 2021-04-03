using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace FromLocalsToLocals.Contracts.Entities
{
    public class AppUser : IdentityUser
    {
        public byte[] Image { get; set; }
        public int VendorsCount { get; set; }

        [JsonIgnore] [IgnoreDataMember] public virtual ICollection<Vendor> Vendors { get; set; }

        [JsonIgnore] [IgnoreDataMember] public virtual ICollection<Follower> Following { get; set; }


        public bool Subscribe { get; set; }

        [JsonIgnore] [IgnoreDataMember] public virtual ICollection<Contact> Contacts { get; set; }
    }
}