using FromLocalsToLocals.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FromLocalsToLocals.Models
{
    public class Post
    {
        public Post()
        { }

        public Post(string text , Vendor vendor, byte[] image = null)
        {
            Vendor = vendor;
            VendorID = vendor.ID;
            Text = text;
            Image = image;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostID { get; set; }

        public int VendorID { get; set; }

        public string Date { get; set; }

        public string Text { get; set; }

        public byte[] Image { get; set; }

        [ForeignKey("VendorID")]
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Vendor Vendor { get; set; }

    }
}
