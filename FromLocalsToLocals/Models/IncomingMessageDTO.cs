using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models
{
    public class IncomingMessageDTO
    {
        public string Message { get; set; }
        public bool IsUserTab { get; set; }
        public int ContactId { get; set; }
    }

    public class OutGoingMessageDTO
    {
        public string Message { get; set; }
        public bool IsUserTab { get; set; }
        public int ContactID { get; set; }
        public byte[] Image { get; set; }
        public string VendorTitle { get; set; }
    }
}
