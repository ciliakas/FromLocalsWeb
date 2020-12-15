using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Web.Models;
using FromLocalsToLocals.Web.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Web.Controllers
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

        public async Task<IActionResult> Index(string tabName ,int vendorId)
        {
            var user = await _userManager.GetUserAsync(User);
            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.ID == vendorId);

            //User cannot chat with vendors that belongs to him
            if (vendorId !=0 && vendor != null && !user.Vendors.Any(x=> x.ID == vendorId))
            {
                var contact = user.Contacts.FirstOrDefault(x => x.ReceiverID == vendorId);
                if(contact != null)
                {
                    return View(Tuple.Create(user, true, contact));
                }

                contact = new Contact(user,vendor,true,false);

                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                return View(Tuple.Create(user, true, contact));   
            }

            return View(Tuple.Create(user,tabName == "ISent",new Contact { ID = -1 }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] IncomingMessageDTO message)
        {
            var user = await _userManager.GetUserAsync(User);
            var userIdToSend = "";
            var outgoingMessage = new OutGoingMessageDTO() { Message = message.Message, ContactID = message.ContactId, IsUserTab = message.IsUserTab };
            Contact contact = null;

            if (message.IsUserTab)
            {
                
                contact = user.Contacts.FirstOrDefault(x => x.ID == message.ContactId);
                if(contact == null)
                {
                    return Json(new{ success = false});
                }
                userIdToSend = contact.Vendor.UserID;
                outgoingMessage.Image = user.Image;
                
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
                        outgoingMessage.Image = x.Image;
                        return;
                    }
                });
            }

            if (contact == null)
            {
                return Json(new { success = false });
            }

            try
            {
                contact.Messages.Add(new Message { Contact = contact, IsUserSender = message.IsUserTab, Text = message.Message });
                contact.ReceiverRead = !message.IsUserTab;
                contact.UserRead = message.IsUserTab;
           
                _context.Update(contact);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }

            outgoingMessage.VendorTitle = contact.Vendor.Title;
            await _hubContext.Clients.User(userIdToSend).SendAsync("sendNewMessage", JsonConvert.SerializeObject(outgoingMessage));
           
            return Json(new { success = true });
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



        public async  Task<IActionResult> GetChatComponent(int contactId, bool isUserTab, string componentName)
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
            if (uContact == null)
            {
                return Json(new { success = false });
            }

            return ViewComponent(componentName, new { contact = uContact, isUserTab });
        }
    }
}
