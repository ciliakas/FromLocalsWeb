using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual AppUser User { get; set; }

        public int ReceiverID { get; set; }
        [ForeignKey("ReceiverID")]
        public virtual Vendor Vendor { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

    }
}
