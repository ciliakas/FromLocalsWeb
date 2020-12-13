using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models.ViewModels
{
    public class MessageVM
    {
    }

    public class ContactsVM
    {
        public string Title { get; set; }
        public byte[] Image { get; set; }

        public Contact Contact { get; set; }

    }
}
