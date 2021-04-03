using FromLocalsToLocals.Contracts.Entities;

namespace FromLocalsToLocals.Web.ViewModels
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