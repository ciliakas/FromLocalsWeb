

namespace FromLocalsToLocals.Models
{
    public class Follower
    {
        public string UserID { get; set; }
        public AppUser User { get; set; }

        public int VendorID { get; set; }
        public Vendor Vendor { get; set; }
    }
}
