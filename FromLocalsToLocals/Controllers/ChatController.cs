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


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(Tuple.Create(user,false));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageDTO message)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return Json(new { success = false });
            }
            
            if ((user.UserName != message.userName && message.isUserSender) || (!(user.Vendors.Any(x => x.Title == message.vendorTitle)) && !message.isUserSender)) {
                return Json(new { success = false });
            }

            Contact contact;
            if (message.isUserSender)
            {
                contact = user.Contacts.FirstOrDefault(x => x.Vendor.Title == message.vendorTitle);
            }
            else
            {
                contact = user.Vendors.FirstOrDefault(x => x.Title == message.vendorTitle).Contacts.FirstOrDefault(x => x.Vendor.Title == message.vendorTitle);
            }


            if (contact == null)
            {
                var newContact = new Contact
                {
                    Vendor = _context.Vendors.FirstOrDefault(x => x.Title == message.vendorTitle),
                    User = user
                };
                var newMsg = new Message
                {
                    Text = message.message,
                    IsUserSender = message.isUserSender,
                    Contact = newContact
                };
            
                _context.Contacts.Add(newContact);
                await _context.SaveChangesAsync();
            }
            else
            {
                try
                {
                    contact.Messages.Add(new Message { Contact = contact, ContactID = contact.ID, IsUserSender = message.isUserSender, Text = message.message });
                    _context.Update(contact);
                    // _context.Messages.Add(new Message { Contact = contact, ContactID = contact.ID, IsUserSender = message.isUserSender, Text = message.message });
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Json(new { success = false });
            
                }
            
            }
            

            return Json(new { success = true });
        }


    }


}
