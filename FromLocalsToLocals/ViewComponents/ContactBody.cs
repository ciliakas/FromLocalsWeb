using FromLocalsToLocals.Contracts.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Web.ViewComponents
{
    public class ContactBody : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Contact contact, bool userTab)
        {
            var isRead = contact.ReceiverRead;
            var title = contact.User.UserName;
            var image = contact.User.Image;
            if (userTab)
            {
                isRead = contact.UserRead;
                title = contact.Vendor.Title;
                image = contact.Vendor.Image;
            }

            if (contact.Messages== null || contact.Messages.Count == 0)
            {
                return View(new ContactBodyVM(contact.ID, title, image, "No Messages", "", true, true));
            }

            var lastMsg = contact.Messages.OrderByDescending(x => x.Date).Take(1).First();

            var contactBodyVM = new ContactBodyVM(contact.ID, title,image,lastMsg.Text,
                                                     lastMsg.Date,isRead,userTab);

            return View(contactBodyVM);
        }

        public class ContactBodyVM
        {
            public ContactBodyVM(int contactId , string title, byte[] image, string lastMsg,string date,bool isRead, bool userTab)
            {
                ContactID = contactId;
                UserTab = userTab;
                Title = title;
                Image = image;
                LastMsg = lastMsg;
                Date = date;
                IsRead = isRead;
            }

            public int ContactID { get; set; }
            public bool UserTab { get; set; }

            public string Title { get; set; }
            public byte[] Image { get; set; }
            public string LastMsg { get; set; }
            public string Date { get; set; }
            public bool IsRead { get; set; }
        }

    }
}
