using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FromLocalsToLocals.Contracts.Entities
{
    public class Listing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListingID { get; set; }

        public int VendorID { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public byte[] Image { get; set; }

        public string Description { get; set; }
    }
}