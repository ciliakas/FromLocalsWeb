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
        public async Task<IViewComponentResult> InvokeAsync(string title,byte[] image,string lastMsg,string date, bool isRead)
        {
            ContactBodyVM vm = new ContactBodyVM
            {
                Title = title,
                Image = image,
                LastMsg = lastMsg,
                Date = date,
                IsRead = isRead
            };

            return View(vm);
        }

        public class ContactBodyVM
        {
            public string Title { get; set; }
            public byte[] Image { get; set; }
            public string LastMsg { get; set; }
            public string Date { get; set; }
            public bool IsRead { get; set; }
        }

    }
}
