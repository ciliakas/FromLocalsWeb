using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using FromLocalsToLocals.Utilities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.ViewComponents
{
    public class ContactBody : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Contact contact,string title,byte[] image, bool userTab)
        {
            var lastMsg = contact.Messages.OrderByDescending(x => x.Date).Take(1).First();
            var isRead = contact.ReceiverRead;
            if (userTab)
            {
                isRead = contact.UserRead;
            }

            ContactBodyVM vm = new ContactBodyVM(contact.ID, title,image,lastMsg.Text,
                                                lastMsg.Date,isRead,userTab);

            return View(vm);
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
