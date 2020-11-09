using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotiId { get; set; }
        public string OwnerId { get; set; }
        public int VendorId { get; set; }
        public bool IsRead { get; set; }
        public string Url { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public string NotiBody { get; set; }

    }
}
