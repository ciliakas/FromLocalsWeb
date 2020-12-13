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
        public Contact()
        { Messages = new List<Message>(); }

        public Contact(AppUser user, Vendor receiver, bool userRead, bool receiverRead)
        {
            User = user;
            Vendor = receiver;
            UserRead = userRead;
            ReceiverRead = receiverRead;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public bool UserRead { get; set; }
        public bool ReceiverRead { get; set; }



        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual AppUser User { get; set; }

        public int ReceiverID { get; set; }
        [ForeignKey("ReceiverID")]
        public virtual Vendor Vendor { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

    }
}
