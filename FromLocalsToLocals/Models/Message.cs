using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Text { get; set; }
         
        public string Date { get; set; }

        public bool IsUserSender { get; set; }

        public int ContactID { get; set; }
        [ForeignKey("ContactID")]
        public virtual  Contact Contact { get; set; }
    }
}
