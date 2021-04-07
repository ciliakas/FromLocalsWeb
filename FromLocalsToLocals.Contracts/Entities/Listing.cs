using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FromLocalsToLocals.Contracts.Entities
{
    public class Listing
    {
        public Listing(int vendorID, string name, decimal price, byte[] image, string description)
        {
            VendorID = vendorID;
            Name = name;
            Price = price;
            Image = image;
            Description = description;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListingID { get; set; }

        public int VendorID { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public byte[] Image { get; set; }

        public string Description { get; set; }

        public void SetListing(Listing listing)
        {
            listing.VendorID = VendorID;
            listing.Name = Name;
            listing.Price = Price;
            listing.Image = Image;
            listing.Description = Description;

        }
    }
}