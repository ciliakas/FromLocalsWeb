using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models
{
    public class MessageDTO
    {
        public string userName { get; set; }
        public string vendorTitle { get; set; }
        public string message { get; set; }
        public bool isUserSender { get; set; }


        public MessageDTO(string userName, string vendorTitle, string message, bool isUserSender)
        {
            this.userName = userName;
            this.vendorTitle = vendorTitle;
            this.message = message;
            this.isUserSender = isUserSender;
        }

    }
}
