using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using FromLocalsToLocals.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private readonly IHubContext<MessageHub> _hubContext;

        public ChatController(UserManager<AppUser> userManager, AppDbContext context, IHubContext<MessageHub> hubContext)
        {
            _userManager = userManager;
            _context = context;
            _hubContext = hubContext;
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
            string userIdToSend = "";

            if(user == null)
            {
                return Json(new { success = false });
            }
            
            Contact contact = null;
            NewMessageDTO dto = new NewMessageDTO() { Text = message.Message, ContactID = message.ContactId, IsUserTab=message.IsUserTab};

            if (message.IsUserTab)
            {
                contact = user.Contacts.FirstOrDefault(x=> x.ID == message.ContactId);
                userIdToSend = contact.Vendor.UserID;
                dto.Image = user.Image;
            }
            else
            {
                user.Vendors.ToList().ForEach(x =>
                {
                    var c = x.Contacts.FirstOrDefault(y => y.ID == message.ContactId);
                    if (c != null)
                    {
                        contact = c;
                        userIdToSend = contact.UserID;
                        dto.Image = x.Image;
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
            
            
            await _hubContext.Clients.User(userIdToSend).SendAsync("sendNewMessage", JsonConvert.SerializeObject(dto));
                     
            return Json(new { success = true });
        }

        private class NewMessageDTO
        {
            public string Text { get; set; }
            public byte[] Image { get; set; }
            public int ContactID { get; set; }
            public bool IsUserTab { get; set; }
        }

        [HttpPost]
        public async Task ReadMessage(int contactId)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.ID == contactId);
            contact.ReceiverRead = true;
            contact.UserRead = true;
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

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
