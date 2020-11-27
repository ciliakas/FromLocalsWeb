using FromLocalsToLocals.Models.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FromLocalsToLocals.Models
{
    public class Post
    {
        public Post()
        { }

        public Post(PostVM vm)
        {
            Vendor = vm.SelectedVendor;
            Text = vm.PostText;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostID { get; set; }

        public int VendorID { get; set; }

        public string Date { get; set; }

        public string Text { get; set; }

        public byte[] Image { get; set; }

        [ForeignKey("VendorID")]
        public Vendor Vendor { get; set; }

    }
}
