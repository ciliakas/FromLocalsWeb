using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public ChatController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string tabName)
        {
            var user = await _userManager.GetUserAsync(User);
            return View(Tuple.Create(user,tabName == "ISent"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageDTO message)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return Json(new { success = false });
            }
            
            Contact contact = null;
            
            if (message.IsUserTab)
            {
                contact = user.Contacts.FirstOrDefault(x=> x.ID == message.ContactId);
            
            }
            else
            {
                user.Vendors.ToList().ForEach(x =>
                {
                    var c = x.Contacts.FirstOrDefault(y => y.ID == message.ContactId);
                    if (c != null)
                    {
                        contact = c;
                        return;
                    }
                });
            }

            if(contact == null || (contact.User == user && !message.IsUserTab))
            {
                return Json(new { success = false });
            }

           try
           {
               contact.Messages.Add(new Message { Contact = contact, ContactID = contact.ID, IsUserSender = message.IsUserTab, Text = message.Message });
                if (message.IsUserTab)
                {
                    contact.ReceiverRead = false;
                }
                else
                {
                    contact.UserRead = false;
                }
                _context.Update(contact);
               await _context.SaveChangesAsync();
           }
           catch (Exception ex)
           {
               return Json(new { success = false });
           
           }
                     
            return Json(new { success = true });
        }

        public async  Task<IActionResult> GetMessagesComponent(int contactId, bool isUserTab)
        {
            var user = await _userManager.GetUserAsync(User);
            Contact uContact = null;
            if (isUserTab)
            {
                uContact = user.Contacts.FirstOrDefault(x => x.ID == contactId);
            }
            else
            {
                user.Vendors.ToList().ForEach(x => {
                    var c =  x.Contacts.FirstOrDefault(y => y.ID == contactId);
                    if(c != null)
                    {
                        uContact = c;
                        return;
                    }

                });
            }

            return ViewComponent("Messages", new { contact = uContact, isUserTab = isUserTab });
        }

    }


}
