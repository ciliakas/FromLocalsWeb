using FromLocalsToLocals.Contracts.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Web.ViewComponents
{
    public class Messages: ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(Contact contact, bool isUserTab)
        {
            if(contact == null)
            {
                return null;
            }

            byte[] image;
            string title;

            if (isUserTab)
            {
                image = contact.Vendor.Image;
                title = contact.Vendor.Title;
            }
            else
            {
                image = contact.User.Image;
                title = contact.User.UserName;
            }

            var vm = new MessagesVM
            {
                IsUserTab = isUserTab,
                Image = image,
                Title = title           
            };

            if (contact.Messages != null)
            {
                vm.Messages = contact.Messages.OrderBy(x => x.Date).ToList();

            }
            else
            {
                vm.Messages = new List<Message>();

            }

            return View("Message", vm);
        }

        public class MessagesVM
        {
            public IEnumerable<Message> Messages { get; set; }
            public bool IsUserTab { get; set; }
            public byte[] Image {get;set;}
            public string Title { get; set; }
        }

    }
}
