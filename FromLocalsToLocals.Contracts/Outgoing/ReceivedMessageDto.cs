using System;
using System.Collections.Generic;
using System.Text;

namespace FromLocalsToLocals.Contracts.Outgoing
{
    class ReceivedMessageDto
    {
        public string Message { get; set; }
        public bool IsUserTab { get; set; }
        public int ContactId { get; set; }
    }
}
