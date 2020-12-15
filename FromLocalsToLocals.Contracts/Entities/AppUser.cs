using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FromLocalsToLocals.Contracts.Entities
{
    public class AppUser : IdentityUser
    {
        public byte[] Image { get; set; }
        public int VendorsCount { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Vendor> Vendors { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Follower> Following { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Contact> Contacts { get; set; }

    }
}
