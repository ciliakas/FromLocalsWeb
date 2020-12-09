using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models
{
    public class MessageDTO
    {
        public string Message { get; set; }
        public bool IsUserTab { get; set; }
        public int ContactId { get; set; }
        public bool NewContact { get; set; }
        public string NewContactVendorTitle { get; set; }
    }
}
